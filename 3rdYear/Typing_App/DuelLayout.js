let layoutBtn = document.querySelector("#layoutBtn");
let currentLayout = localStorage.getItem("layout") ?? "row";
let duelSection = document.querySelector("#duelSection");
setLayout();

function changeLayout() {
    layoutBtn = document.querySelector("#layoutBtn");
    currentLayout = localStorage.getItem("layout") ?? "row";
    duelSection = document.querySelector("#duelSection");

    currentLayout = currentLayout == "row" ? "col" : "row";
    setLayout();
}

function setLayout() {
    if (currentLayout == "col") {
        layoutBtn.classList.remove("row")
        layoutBtn.classList.add(currentLayout);
        duelSection.classList.add("col");
    } else {
        currentLayout = "row";
        layoutBtn.classList.remove("col")
        layoutBtn.classList.add(currentLayout);  
        duelSection.classList.remove("col");
    }

    localStorage.setItem("layout", currentLayout);
}