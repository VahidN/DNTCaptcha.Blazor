var DntCaptcha;
(function (DntCaptcha) {
    class DntCanvasCaptcha {
        constructor() {
            this.canvas = undefined;
            this.options = undefined;
            this.ctx = null;
            this.font = undefined;
            this.interval = null;
        }
        startTimer(func) {
            if (!this.options) {
                return;
            }
            func();
            this.interval = setInterval(func, this.options.timerInterval);
        }
        stopTimer() {
            if (!this.interval) {
                return;
            }
            clearInterval(this.interval);
        }
        randomNumber(min, max) {
            return Math.random() * (max - min) + min;
        }
        randomIntNumber(min, max) {
            return Math.floor(Math.random() * (max - min) + min);
        }
        randomColor(min, max) {
            const r = this.randomIntNumber(min, max);
            const g = this.randomIntNumber(min, max);
            const b = this.randomIntNumber(min, max);
            return `rgb(${r}, ${g}, ${b})`;
        }
        setCanvasSize() {
            if (!this.ctx || !this.options || !this.canvas || !this.font) {
                return;
            }
            this.ctx.font = this.font;
            const metrics = this.ctx.measureText(this.options.text);
            const actualHeight = metrics.actualBoundingBoxAscent + metrics.actualBoundingBoxDescent;
            this.canvas.height = actualHeight + 2 * this.options.padding;
            this.canvas.width = metrics.width + 2 * this.options.padding;
        }
        drawBackgroundColor() {
            if (!this.ctx || !this.options || !this.canvas) {
                return;
            }
            const x = 0;
            const y = 0;
            const width = this.canvas.width;
            const height = this.canvas.height;
            this.ctx.save();
            this.ctx.lineJoin = "round";
            this.ctx.lineWidth = this.options.borderWidth;
            this.ctx.fillStyle = this.options.backgroundColor;
            this.ctx.fillRect(x, y, width, height);
            this.ctx.fill();
            this.ctx.strokeStyle = this.options.borderColor;
            this.ctx.strokeRect(x, y, width, height);
            this.ctx.stroke();
            this.ctx.restore();
        }
        drawText() {
            if (!this.ctx || !this.options || !this.canvas || !this.font) {
                return;
            }
            this.ctx.fillStyle = this.options.fontColor;
            this.ctx.font = this.font;
            const x = this.options.padding;
            const y = this.randomIntNumber(0 + 2 * this.options.padding, this.canvas.height - this.options.padding);
            const deg = this.randomIntNumber(-2, 2);
            this.ctx.save();
            this.ctx.translate(x, y);
            this.ctx.shadowBlur = this.options.shadowBlur;
            this.ctx.shadowColor = this.options.shadowColor;
            this.ctx.shadowOffsetX = this.options.shadowOffsetX;
            this.ctx.shadowOffsetY = this.options.shadowOffsetY;
            this.ctx.rotate((deg * Math.PI) / 180);
            this.ctx.fillText(this.options.text, 0, 0);
            this.ctx.restore();
        }
        drawInterferenceLines() {
            if (!this.ctx || !this.options || !this.canvas) {
                return;
            }
            for (let i = 0; i < this.options.randomLinesCount; i++) {
                this.ctx.save();
                this.ctx.strokeStyle = this.randomColor(40, 180);
                this.ctx.globalAlpha = this.randomNumber(0.5, 1);
                this.ctx.beginPath();
                this.ctx.moveTo(this.randomIntNumber(0, this.canvas.width), this.randomIntNumber(0, this.canvas.height));
                this.ctx.lineTo(this.randomIntNumber(0, this.canvas.width), this.randomIntNumber(0, this.canvas.height));
                this.ctx.stroke();
                this.ctx.restore();
            }
        }
        drawInterferenceCircles() {
            if (!this.ctx || !this.options || !this.canvas) {
                return;
            }
            for (let i = 0; i < this.options.randomCirclesCount; i++) {
                this.ctx.save();
                this.ctx.strokeStyle = this.randomColor(40, 180);
                this.ctx.globalAlpha = this.randomNumber(0.5, 1);
                this.ctx.beginPath();
                this.ctx.arc(this.randomIntNumber(0, this.canvas.width), this.randomIntNumber(0, this.canvas.height), this.randomIntNumber(0, this.canvas.height / 2), Math.PI * 2, 0, false);
                this.ctx.stroke();
                this.ctx.closePath();
                this.ctx.restore();
            }
        }
        drawNoisePoints() {
            if (!this.ctx || !this.options || !this.canvas) {
                return;
            }
            for (let i = 0; i < this.options.noisePointsCount; i++) {
                this.ctx.save();
                this.ctx.fillStyle = this.randomColor(0, 255);
                this.ctx.beginPath();
                this.ctx.arc(this.randomIntNumber(0, this.canvas.width), this.randomIntNumber(0, this.canvas.height), 1, 0, 2 * Math.PI);
                this.ctx.fill();
                this.ctx.restore();
            }
        }
        createContext() {
            if (!this.canvas) {
                return;
            }
            this.ctx = this.canvas.getContext("2d");
            if (this.ctx) {
                this.ctx.textBaseline = "bottom";
            }
        }
        drawCaptcha(element, options) {
            this.canvas = element;
            this.options = options;
            if (!element || !options) {
                return;
            }
            this.font = `${options.fontSize}px "${options.fontName}"`;
            this.createContext();
            this.stopTimer();
            this.startTimer(() => {
                this.setCanvasSize();
                this.drawBackgroundColor();
                this.drawText();
                this.drawInterferenceLines();
                this.drawInterferenceCircles();
                this.drawNoisePoints();
            });
        }
    }
    DntCaptcha.DntCanvasCaptcha = DntCanvasCaptcha;
})(DntCaptcha || (DntCaptcha = {}));
const captchaElements = {};
export function drawDntCaptcha(element, options) {
    if (!element || !element.id) {
        return;
    }
    let captcha = captchaElements[element.id];
    if (!captcha) {
        captcha = new DntCaptcha.DntCanvasCaptcha();
        captcha.drawCaptcha(element, options);
        captchaElements[element.id] = captcha;
    }
    else {
        captcha.drawCaptcha(element, options);
    }
}
