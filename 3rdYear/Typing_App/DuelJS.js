let gameWords = [];
let currentWordIndex = 0;
let currentLetterIndex = 0;
let gameActive = false;
let timerStarted = false;  // flag to check if the timer has started
let startTime = null;
let correctCharacters = 0;
let totalCharacters = 0;
let timerInterval;
let accuracyScore = 100;
let errorCount = 0;
let enemyWPM = 0;
document.addEventListener('keydown', handleKeyPress);
window.addEventListener("beforeunload", () => {
    if (conn) conn.close();
    peer.destroy();
});

let peerId = generateId()
const peer = new Peer(peerId, {
    host: '',
    path: '/peerjs',
    secure: true
}); // Create PeerJS instance
let connected = false; // Track if a player is already connected
let conn;
let score = 0;

const Status = document.getElementById("status");
const peerIdSpan = document.getElementById("peer-id");
const peerInput = document.getElementById("peer-input");
const connectForm = document.getElementById("connect-form");
const enemyDisplay = document.getElementById("enemyDisplay");
const wordDisplay = document.getElementById("myDisplay");
const countdownDialog = document.getElementById('countdown');
const codeDialog = document.getElementById('codeDialog');
const peerCodeBtn = document.getElementById('peer-codeBtn');

codeDialog.showModal();
countdownDialog.addEventListener('cancel', (event) => {
    event.preventDefault();
});

// Show PeerJS ID
peer.on("open", (id) => {
    if (peerIdSpan)
        peerIdSpan.textContent = id;
});

// When Player 1 receives a connection
peer.on("connection", (connection) => {
    if (connected) {
        console.log("Connection attempt rejected: Room is full.");
        connection.send({ type: "error", message: "Room is full." });
        connection.close(); // Reject additional players
        return;
    }

    // Accept the connection if no one is connected
    conn = connection;
    connected = true;
    setupConnection();
});

// Player 2 connects to Player 1

if (connectForm)
    connectForm.addEventListener("submit", (e) => {
        e.preventDefault();
        if (conn && conn.open) {
            alert("You are already connected!");
            return;
        }

        const remoteId = peerInput.value.trim();
        if (!remoteId || remoteId === peerId || remoteId.length > 6) {
            alert("Please enter a valid ID!");
            return;
        }

        conn = peer.connect(remoteId);
        conn.on("open", () => {
            Status.textContent = "Connected to: " + remoteId;
            setupConnection();
        });

        conn.on("data", (data) => {
            if (data.type === "error" && data.message === "Room is full.") {
                alert("Room is full. Try again later.");
                conn.close();
            }
        });

        conn.on("error", (err) => {
            Status.textContent = "Connection failed: " + err.message;
            if (peerInput)
                peerInput.value = ""
        });
        return false;
    });

function setupConnection() {
    codeDialog.close();
    peerCodeBtn.disabled = true;
    Status.textContent = "Connected! Get ready...";

    if (conn.open) {
        conn.send({
            type: "ready"
        });
    }

    // Show countdown to both players
    startCountdown(5);
    if (peerInput) peerInput.disabled = true;
    if (connectForm) {
        connectForm.disabled = true;
        connectForm.inert = true;
    }

    if (conn.open) {
        conn.send({
            type: "init",
            words: gameWords,
            currentWordIndex,
            currentLetterIndex,
            startTime: startTime || new Date()
        });
    }

    conn.on("data", handleIncomingData);

    conn.on("close", () => {
        console.log("Opponent disconnected.");
        Status.textContent = "Opponent disconnected. Waiting for new player...";

        peerCodeBtn.disabled = false;
        codeDialog.showModal();

        // Reset connection variables
        conn = null; // Ensure no lingering connection reference
        connected = false;
        if (peerInput) {
            peerInput.value = "";
            peerInput.disabled = false;
        }

        if (connectForm) {
            connectForm.disabled = false;
            connectForm.inert = false;
        }

        // Reset the game state if the opponent leaves
        resetGame();
    });
}

function handleIncomingData(data) {
    if (data.type === "enemyText") {
        enemyDisplay.innerHTML = data.text.length ? data.text : '...';

    }

    // Get opponents wpm for comparing
    if (data.type === "enemyWPM") {
        enemyWPM = data.wpm;
    }

    // Synchronize initial state for the connecting player
    if (data.type === "init") {
        gameWords = data.words.length ? data.words : '...';
        currentWordIndex = data.currentWordIndex;
        currentLetterIndex = data.currentLetterIndex;

        // Sync timer
        startTime = new Date(data.startTime);
        gameActive = true;

        displayWords();
        updateTimer();
    }

    if (data.type === "ready") {
        startCountdown(5);
    }
}

function startGame() {
    fetch(APP_URL + '/Typing/StartGame', {
        method: "GET",
        headers: { "Accept": "application/json" },
        mode: "cors",
        credentials: "include"
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                gameWords = data.words;
                currentWordIndex = 0;
                currentLetterIndex = 0;
                correctCharacters = 0;
                totalCharacters = gameWords.join('').length;
                accuracyScore = 100;
                errorCount = 0;
                gameActive = true;
                startTime = new Date(); // Set common start time
                displayWords();
                updateTimer();

                // Share start time and initial words with the opponent
                if (conn && conn.open) {
                    conn.send({
                        type: "init",
                        words: gameWords,
                        currentWordIndex,
                        currentLetterIndex,
                        startTime: startTime
                    });
                }
            }
        });
}

function displayWords() {
    let text = gameWords.map((word, wordIndex) =>
        `<span class="word ${wordIndex === currentWordIndex ? 'current-word' : ''}" id="word-${wordIndex}">
                    ${word.split('').map((letter, letterIndex) =>
            `<span id="letter-${wordIndex}-${letterIndex}">${letter}</span>`
        ).join('')}
                </span>`).join(' ');

    wordDisplay.innerHTML = text;

    if (conn) {
        conn.send({ type: "enemyText", text });
    }
}

function updateWordsDisplay() {
    // dynamically update the word display to include any new words
    let text = gameWords.map((word, wordIndex) =>
        `<span class="word ${wordIndex === currentWordIndex ? 'current-word' : ''}" id="word-${wordIndex}">
                    ${word.split('').map((letter, letterIndex) =>
            `<span id="letter-${wordIndex}-${letterIndex}">${letter}</span>`
        ).join('')}
                </span>`).join(' ');

    wordDisplay.innerHTML = text;

    if (conn) {
        conn.send({ type: "enemyText", text });
    }
}
function addMoreWords() {
    fetch(APP_URL + '/Typing/LoadMoreWords', {
        method: "GET",
        headers: { "Accept": "application/json" },
        mode: "cors",
        credentials: "include"
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                const startIndex = gameWords.length;
                gameWords.push(...data.words);

                // append new words without overwriting the current display
                wordDisplay.innerHTML += data.words.map((word, wordIndex) =>
                    `<span class="word" id="word-${startIndex + wordIndex}">
                                ${word.split('').map((letter, letterIndex) =>
                        `<span id="letter-${startIndex + wordIndex}-${letterIndex}">${letter}</span>`
                    ).join('')}
                            </span>`
                ).join(' ');

                // ensure the current word remains highlighted
                const currentWordElement = wordDisplay.querySelector(`#word-${currentWordIndex}`);
                if (currentWordElement) {
                    currentWordElement.classList.add('current-word');
                }

                let text = wordDisplay.innerHTML;

                if (conn) {
                    conn.send({ type: "enemyText", text });
                }
            }
        });
}

function updateTimer() {
    const timerElement = document.getElementById("timer");
    const totalTime = 30;

    if (timerInterval) clearInterval(timerInterval);

    timerInterval = setInterval(() => {
        const elapsed = Math.floor((new Date() - startTime) / 1000);
        const remaining = totalTime - elapsed;

        if (remaining <= 0) {
            clearInterval(timerInterval);
            endGame();
        } else {
            timerElement.textContent = `${remaining}s Left`;
        }
    }, 1000);
}

function handleKeyPress(e) {

    if (!gameActive) return;

    let text = wordDisplay.innerHTML;

    if (conn) {
        conn.send({ type: "enemyText", text });
    }

    // start timer if not already started
    if (!timerStarted && e.key.length === 1 && !e.metaKey && !e.ctrlKey && !e.altKey) {
        timerStarted = true;
        startTime = new Date();
        updateTimer();
    }

    // handle backspace
    if (e.key === 'Backspace') {
        if (currentLetterIndex > 0) {
            currentLetterIndex--;
            const letterElement = wordDisplay.querySelector(`#letter-${currentWordIndex}-${currentLetterIndex}`);
            if (letterElement) {
                // Decrease accuracy when going back to fix an error
                if (letterElement.classList.contains('incorrect-letter')) {
                    errorCount++;
                    accuracyScore = calculateAccuracy();
                }
                letterElement.classList.remove('correct-letter', 'incorrect-letter');
            }
        }
        return;
    }

    // handle space-bar for next word
    if (e.key === ' ') {
        const currentWordElement = wordDisplay.querySelector(`#word-${currentWordIndex}`);
        if (!currentWordElement) {
            console.warn(`current word element not found for index ${currentWordIndex}`);
            return;
        }

        // mark current word as completed and track errors
        for (let i = 0; i < currentWordElement.children.length; i++) {
            const letterElement = wordDisplay.querySelector(`#letter-${currentWordIndex}-${i}`);
            if (letterElement && !letterElement.classList.contains('correct-letter')) {
                letterElement.classList.add('incorrect-letter');
                // Decrease accuracy for incomplete/incorrect letters in the word
                accuracyScore = calculateAccuracy();
                errorCount++;
            }
        }

        currentWordElement.classList.remove('current-word');
        currentWordIndex++;
        currentLetterIndex = 0;

        // load more words if needed
        if (currentWordIndex >= gameWords.length) {
            addMoreWords();
        }

        const nextWordElement = wordDisplay.querySelector(`#word-${currentWordIndex}`);
        if (nextWordElement) {
            nextWordElement.classList.add('current-word');
        }

        e.preventDefault();
        return;
    }

    // handle letter input
    if (e.key.length === 1 && !e.metaKey && !e.ctrlKey && !e.altKey) {
        const currentWord = gameWords[currentWordIndex];

        // ensure valid indices
        if (!currentWord || currentLetterIndex >= currentWord.length) return;

        const currentLetter = currentWord[currentLetterIndex];
        const letterElement = wordDisplay.querySelector(`#letter-${currentWordIndex}-${currentLetterIndex}`);
        if (!letterElement) {
            console.warn(`letter element not found for word ${currentWordIndex}, letter ${currentLetterIndex}`);
            return;
        }

        if (e.key.toLowerCase() === currentLetter.toLowerCase()) {
            letterElement.classList.add('correct-letter');
            correctCharacters++;
        } else {
            letterElement.classList.add('incorrect-letter');
            // Decrease accuracy for incorrect letter
            accuracyScore = calculateAccuracy();
            errorCount++;
        }

        currentLetterIndex++;
    }
}

function endGame() {
    gameActive = false;

    const endTime = new Date();
    const timeTakenSeconds = (endTime - startTime) / 1000;
    const wpm = Math.round((correctCharacters / 5) / (timeTakenSeconds / 60));

    const accuracy = Math.round(accuracyScore);

    // Send WPM to opponent (if connected)
    if (conn && conn.open) {
        conn.send({ type: "enemyWPM", wpm });
    }

    // Delay redirect to allow PeerJS to send the message
    setTimeout(() => {
        const isWinner = wpm >= enemyWPM;

        // Redirect to the statistics page
        window.location.href = `${APP_URL}/Duel/Results?isWinner=${isWinner}&wpm=${wpm}&accuracy=${accuracy}`;
    }, 2000); // Ensure enough time to send data
}

function resetGame() {
    enemyDisplay.innerHTML = '...';
    wordDisplay.innerHTML = '...';
    gameWords = [];
    currentWordIndex = 0;
    currentLetterIndex = 0;
    correctCharacters = 0;
    totalCharacters = 0;
    accuracyScore = 100;
    errorCount = 0;
    gameActive = false;
    clearInterval(timerInterval);
    codeDialog.showModal();
    peerCodeBtn.disabled = false;
    timerStarted = false;
    startTime = null;
    displayWords();
}

function calculateAccuracy() {
    if (totalCharacters === 0) return 100; // Avoid division by zero
    return Math.max(0, ((correctCharacters / totalCharacters) * 100).toFixed(2));
}

function startCountdown(seconds) {
    wordDisplay.innerHTML = '...';
    enemyDisplay.innerHTML = '...';
    countdownDialog.showModal();
    let countdownDiv = countdownDialog.querySelector("h1");

    let timeLeft = seconds;
    countdownDiv.textContent = `${timeLeft}...`;

    const countdownInterval = setInterval(() => {
        timeLeft--;
        countdownDiv.textContent = (timeLeft <= 0 ? `GO!` : `${timeLeft}...`);

        if (timeLeft < 0) {            
            clearInterval(countdownInterval);
            countdownDialog.close();

            Status.textContent = "Connected! Go!";
            if (!gameActive) {
                startGame(); // Start the game once countdown ends
            }
        }
    }, 1000);
}


function generateId() {
    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = '';
    for (let i = 0; i < 6; i++) {
        result += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return result;
}