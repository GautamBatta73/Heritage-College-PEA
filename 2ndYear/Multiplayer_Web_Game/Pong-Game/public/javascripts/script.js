let isReferee = false;

// Canvas Related 
const canvas = document.createElement('canvas');
const context = canvas.getContext('2d');
const socket = io(`/pong`);
let paddleIndex = 0;

let width = 500;
let height = 700;

// Paddle
let paddleHeight = 10;
let paddleWidth = 50;
let paddleDiff = 25;
let paddleX = [225, 225];
let trajectoryX = [0, 0];
let playerMoved = false;

// Ball
let ballX = 250;
let ballY = 350;
let ballRadius = 10;
let ballDirection = 1;

// Speed
let speedY = 15;
let speedX = 0;

// Score for Both Players
let score = [0, 0];

// Consecutive Losses
let consecutiveLosses = 0;
let maxConsecutiveLosses = 3;

// Create Canvas Element
function createCanvas() {
	canvas.id = 'canvas';
	canvas.width = width;
	canvas.height = height;
	document.body.appendChild(canvas);
	renderCanvas();
}

//Wait for Opponents
function renderIntro() {
	// Canvas Background
	context.fillStyle = 'black';
	context.fillRect(0, 0, width, height);

	// Intro Text
	context.fillStyle = 'cyan';
	context.font = "32px Courier New";
	context.fillText("Waiting for opponent...", 20, (canvas.height / 2) - 30);
}

// Render Everything on Canvas
function renderCanvas() {
	// Canvas Background
	context.fillStyle = 'black';
	context.fillRect(0, 0, width, height);

	// Paddle Color
	context.fillStyle = 'cyan';

	// Bottom Paddle
	context.fillRect(paddleX[0], height - 20, paddleWidth, paddleHeight);

	// Top Paddle
	context.fillRect(paddleX[1], 10, paddleWidth, paddleHeight);

	// Dashed Center Line
	context.beginPath();
	context.setLineDash([4]);
	context.moveTo(0, 350);
	context.lineTo(500, 350);
	context.strokeStyle = 'yellow';
	context.stroke();

	// Ball
	context.beginPath();
	context.arc(ballX, ballY, ballRadius, 2 * Math.PI, false);
	context.fillStyle = 'magenta';
	context.fill();

	// Score
	context.font = "32px Courier New";
	context.fillStyle = 'cyan';
	context.fillText(score[0], 20, (canvas.height / 2) + 50);
	context.fillText(score[1], 20, (canvas.height / 2) - 30);
}

// Reset Ball to Center
function ballReset() {
	ballX = width / 2;
	ballY = height / 2;
	speedY = 15;

	if (consecutiveLosses >= maxConsecutiveLosses) {
		ballDirection = -ballDirection;
		consecutiveLosses = 0;
	} else
		consecutiveLosses++;

	socket.emit('ballMove', {
		ballX,
		ballY,
		score
	});
}

// Adjust Ball Movement
function ballMove() {
	// Vertical Speed
	ballY += speedY * ballDirection;
	// Horizontal Speed
	if (playerMoved) {
		ballX += speedX;
	}

	socket.emit(`ballMove`, {
		ballX,
		ballY,
		score
	});
}

// Determine What Ball Bounces Off, Score Points, Reset Ball
function ballBoundaries() {
	// Bounce off Left Wall
	if (ballX < 0 && speedX < 0) {
		speedX = -speedX;
	}
	// Bounce off Right Wall
	if (ballX > width && speedX > 0) {
		speedX = -speedX;
	}
	// Bounce off player paddle (bottom)
	if (ballY > height - paddleDiff) {
		if (ballX >= paddleX[0] && ballX <= paddleX[0] + paddleWidth) {
			// Add Speed on Hit
			if (playerMoved) {
				speedY += 1;
				// Max Speed
				if (speedY > 25) {
					speedY = 15;
				}
			}
			ballDirection = -ballDirection;
			trajectoryX[0] = ballX - (paddleX[0] + paddleDiff);
			speedX = trajectoryX[0] * 0.3;
		} else {
			// Reset Ball, add to Computer Score
			ballReset();
			score[1]++;
		}
	}
	// Bounce off computer paddle (top)
	if (ballY < paddleDiff) {
		if (ballX >= paddleX[1] && ballX <= paddleX[1] + paddleWidth) {
			// Add Speed on Hit
			if (playerMoved) {
				speedY += 1;
				// Max Speed
				if (speedY > 25) {
					speedY = 15;
				}
			}
			ballDirection = -ballDirection;
			trajectoryX[1] = ballX - (paddleX[1] + paddleDiff);
			speedX = trajectoryX[1] * 0.3;
		} else {
			// Reset Ball, Increase Computer Difficulty, add to Player Score
			ballReset();
			score[0]++;
		}
	}
}

// Called Every Frame
function animate() {
	if (isReferee) {
		ballMove();
		ballBoundaries();
	}

	renderCanvas();
	window.requestAnimationFrame(animate);
}

// Load Game, Reset Everything 
function loadGame() {
	createCanvas();
	renderIntro();
	socket.emit('ready')
}

function startGame() {
	paddleIndex = isReferee ? 0 : 1;
	window.requestAnimationFrame(animate);
	canvas.requestPointerLock = canvas.requestPointerLock || canvas.mozRequestPointerLock;
	canvas.style.cursor = 'none';

	canvas.addEventListener('click', () => {
		canvas.requestPointerLock();
	});

	let updatePosition = function (e) {
		playerMoved = true;
		let movementX = e.movementX || e.mozMovementX || 0;

		paddleX[paddleIndex] += movementX;
		if (paddleX[paddleIndex] < 0) {
			paddleX[paddleIndex] = 0;
		} else if (paddleX[paddleIndex] > canvas.width - paddleWidth) {
			paddleX[paddleIndex] = canvas.width - paddleWidth;
		}

		socket.emit(`paddleMove`, {
			xPosition: paddleX[paddleIndex]
		});
	}

	document.addEventListener('pointerlockchange', () => {
		if (document.pointerLockElement === canvas)
			document.addEventListener("mousemove", updatePosition);
		else
			document.removeEventListener("mousemove", updatePosition);
	});

	//For Mobile
	let prevTouchX = null;
	let canvasRect = canvas.getBoundingClientRect();

	canvas.addEventListener('touchstart', (e) => {
		e.preventDefault();
		const touch = e.changedTouches[0];
		prevTouchX = touch.clientX - canvasRect.left;
		playerMoved = true;
	});

	canvas.addEventListener('touchmove', (e) => {
		e.preventDefault();
		const touch = e.changedTouches[0];
		if (prevTouchX) {
			const touchX = touch.clientX - canvasRect.left;
			const diffX = touchX - prevTouchX;
			paddleX[paddleIndex] += diffX;
			if (paddleX[paddleIndex] < 0) {
				paddleX[paddleIndex] = 0;
			} else if (paddleX[paddleIndex] > canvas.width - paddleWidth) {
				paddleX[paddleIndex] = canvas.width - paddleWidth;
			}
			socket.emit(`paddleMove`, {
				xPosition: paddleX[paddleIndex]
			});
		}
		prevTouchX = touch.clientX - canvasRect.left;
	});

	canvas.addEventListener('touchend', () => {
		prevTouchX = null;
		playerMoved = false;
	});
}


// On Load
loadGame();
socket.on(`connect`, () => {
	console.log(`Connected as … ${socket.id}`);
});

socket.on(`startGame`, (refereeId) => {
	console.log(`Referee is ${refereeId}`);
	if (socket.id === refereeId)
		isReferee = true;
	startGame();
});

socket.on(`paddleMove`, (paddleData) => {
	// Toggle 1 to 0, and 0 to 1
	const opponentPaddleIndex = 1 - paddleIndex;
	paddleX[opponentPaddleIndex] = paddleData.xPosition;
});

socket.on(`ballMove`, (ballData) => {
	({ ballX, ballY, score } = ballData);
});