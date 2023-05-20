const main = document.querySelector(`#animationMain`);
const skipBtn = document.querySelector(`#skipBtn`)
const canvas = document.querySelector(`#myCanvas`);
const canvasContext = canvas.getContext(`2d`);

skipBtn.addEventListener(`click`, leave);

let img = new Image();
img.src = `./images/logo.svg`;
img.addEventListener(`load`, () => {
	setTimeout(() => {
		canvasContext.imageSmoothingEnabled = false;
		canvasContext.drawImage(img, 5, 0, 769, 150);
	}, 1350)
}, false);

setTimeout(() => {
	let mainTop = 0;
	let slide = setInterval(() => {
		mainTop--;
		main.style.top = `${mainTop}in`;
		if (mainTop <= -50) {
			clearInterval(slide);
			leave();
		}
	}, 20);
}, 2000)

function leave() {
	location.href = `intro.html`;
}