let stars = [];
let indexes = 0;
let lastPosX = 0;
let lastPosY = 0;
const max_steps = 20;
let left = false;

class Star {
	steps = 0;
	html_item = "";
	angle;
	rotation_angle;
	x = 0;
	y = 0;
	velocityX = 0;
	velocityY = 0;
	faded = false;
	fadedSteps = 0;
	mx_steps;

	inleft = false;

	constructor(pos, angle_c, angle_change, need_to_left, m_steps, speed=4) {
		this.add (pos, angle_c, angle_change, need_to_left, m_steps, speed);
	}

	add(pos, angle_c, angle_change, need_to_left, m_steps, speed) {
		stars.push (this);
		let item = `star_${ indexes }`;
		this.html_item = item;
		this.mx_steps = m_steps;
		indexes++;
		this.inleft = left;
		if (need_to_left) {
			this.angle = angle_c - (this.inleft ? angle_change : -angle_change);
		}
		else {
			this.angle = angle_c - angle_change;
		}
		left = !left;
		let size = Math.random () + 0.6;
		let sizeSVG = Math.min(size, 1.4) * 16;
		let color = '#' + (Math.random () * 0xFFFFFF << 0).toString (16).padStart (6, '0');
		$ ('#star_container').append (`
			<div class="star" id="${ item }">
				<svg xmlns="http://www.w3.org/2000/svg" width="${ sizeSVG }" height="${ sizeSVG }" fill="${ color }" class="bi bi-star-fill" viewBox="0 0 16 16">
					<path d="M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z"/>
				</svg>
			</div>
		`);
		let cy = pos.y;
		let cx = pos.x;
		this.y = cy;
		this.x = cx;
		this.rotation_angle = Math.round((Math.random() + 0.1) * 270);
		let rotation = `rotate(${this.rotation_angle}deg)`;
		/*console.log(rotation);*/
		this.setVelocity ((Math.abs (1.4 - size) + 0.4) * speed);
		$ ("#" + item).css ({
			top: cy,
			left: cx,
			filter: `drop-shadow(0 0 5px ${color})`,
			transform: rotation,
		});
	}

	setVelocity(length) {
		// length = typeof length !== 'undefined' ? length : 10;
		// angle = angle * Math.PI / 180; // if you're using degrees instead of radians
		this.velocityX = length * Math.cos (this.angle);
		this.velocityY = length * Math.sin (this.angle);
	}

	update() {
		if (this.faded && this.fadedSteps >= stepsToHide) {
			this.destroy ();
		} else {
			if (this.steps > this.mx_steps){
				if (!this.faded)
					this.fade();
				else
					this.fadedSteps++;
			}
			this.updateLocation ();
		}
	}

	fade(){
		this.faded = true;
		$ ("#" + this.html_item).css ({
			opacity: 0,
		});
	}

	updateLocation() {
		this.x += this.velocityX;
		this.y += this.velocityY;
		this.rotation_angle += this.inleft ? -5 : 5;
		let rotation = `rotate(${this.rotation_angle}deg)`;


		$ ("#" + this.html_item).css ({
			top: this.y,
			left: this.x,
			transform: rotation,
		});

		this.steps++;
	}

	destroy() {
		let html = this.html_item;
		stars = stars.filter (function (e) { return e.html_item !== html });

		let el = document.getElementById (this.html_item);

		document.getElementById ('star_container').removeChild (el);
	}

}

let start = Date.now ();
const refreshRate = 40;
const stepsToHide = 300 / 40;
document.addEventListener ("mousemove", function (e) {
	const millis = Date.now () - start;
	if (millis >= refreshRate) {
		start = Date.now ();
		const p1 = {
			x: e.clientX ,
			y: e.clientY
		};
		const delta = Math.abs (p1.x - lastPosX) + Math.abs (p1.y - lastPosY);
		let canAdd = true;
		if (delta < 10) {
			canAdd = millis >= refreshRate * 2;
		}
		if (canAdd) {
			const angleDeg = Math.atan2 (lastPosY - p1.y, lastPosX - p1.x) * 180 / Math.PI;
			lastPosX = e.clientX ;
			lastPosY = e.clientY;
			new Star (p1, Math.round (angleDeg), 90, true, max_steps);
		}
	}
});

function Click(e){
	e = e || window.event;
	const p1 = {
		x: e.clientX ,
		y: e.clientY
	};
	const angleDeg = Math.atan2 (lastPosY - p1.y, lastPosX - p1.x) * 180 / Math.PI;
	for (let i = 1; i < 25; i++) {
		let spd = (Math.random() + 0.5) * 4;
		new Star (p1, Math.round (angleDeg), 15 * i, true, 30, spd);
	}

}

function updateStars() {
	setTimeout (function () {
		if (stars.length > 0) {
			for (let star of stars) {
				star.update ();
			}
		}
		updateStars ();
	}, refreshRate);
}

updateStars ();

