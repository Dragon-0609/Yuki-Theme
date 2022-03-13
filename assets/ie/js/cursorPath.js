let stars = [];
let indexes = 0;
let lastPosX = 0;
let lastPosY = 0;
const max_steps = 20;
let left = false;
let mx_steps = new Map ();
let inleft = new Map ();
let angle = new Map ();
let rotation_angle = new Map ();
let velocity = new Map ();
let x_y = new Map ();
let steps = new Map ();
let faded = new Map ();
let fadedSteps = new Map ();


function add(pos, angle_c, angle_change, need_to_left, m_steps, speed) {
	let item = "star_" + indexes;
	stars.push (item);
	mx_steps.set (item, m_steps);
	indexes++;
	inleft.set (item, left);
	faded.set (item, false);
	steps.set (item, 0);
	fadedSteps.set (item, 0);
	if (need_to_left) {
		angle.set (item, angle_c - (inleft ? angle_change : -angle_change));
	} else {
		angle.set (item, angle_c - angle_change);
	}
	left = !left;
	let size = Math.random () + 0.6;
	let sizeSVG = Math.min (size, 1.4) * 16;
	let color = "#000000".replace(/0/g,function(){return (~~(Math.random()*16)).toString(16);});
	let svg_star = document.createElement('div');
	svg_star.classList.add("star");
	svg_star.id = item;
	svg_star.innerHTML = '	<svg xmlns="http://www.w3.org/2000/svg" width="' + sizeSVG + '" height="' + sizeSVG + '" fill="' + color + '" class="bi bi-star-fill" viewBox="0 0 16 16"> ' +
		'		<path d="M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z"/> ' +
		'	</svg> ';
	let cont = document.getElementById('star_container');
	cont.appendChild(svg_star);
	let cy = pos.y;
	let cx = pos.x;
	x_y.set (item, {'x': cx, 'y': cy});
	rotation_angle.set(item, Math.round ((Math.random () + 0.1) * 270));
	let rotation = "rotate(" + rotation_angle.get(item) + "deg)";
	svg_star.style.transform = rotation;
		/*console.log(rotation);*/
	setVelocity (item, (Math.abs (1.4 - size) + 0.4) * speed);
	$ ("#" + item).css ({
		top: cy,
		left: cx,
	});
}

function setVelocity(itm, length) {
	// length = typeof length !== 'undefined' ? length : 10;
	// angle = angle * Math.PI / 180; // if you're using degrees instead of radians
	velocity.set (itm, {x: length * Math.cos (angle.get(itm)), y: length * Math.sin (angle.get(itm))});
}

function update(itm) {
	if(faded.get(itm) && fadedSteps.get(itm) >= stepsToHide){
		destroy (itm);
	} else{ 
		if (steps.get(itm) > mx_steps.get(itm)) {
			if (!faded.get(itm))
				fade(itm);
			else
				fadedSteps.set(itm,fadedSteps.get(itm)+1);
		}
		updateLocation (itm);
	}
}

function fade(itm){
	faded.set(itm, true);
	$ ("#" + itm).css ({
		opacity: 0,
	});
}

function updateLocation(itm) {
	let cm = x_y.get(itm);
	let cxm = velocity.get(itm);
	// console.log(cm);
	cm.x += cxm.x;
	cm.y += cxm.y;
	x_y.set(itm, cm);
	rotation_angle.set(itm, rotation_angle.get(itm) + (inleft.get(itm) ? -5 : 5));
	let rotation = "rotate(" + rotation_angle.get(itm) + "deg)";

	$ ("#" + itm).css ({
		top: cm.y,
		left: cm.x,
		transform: rotation,
	});

	steps.set(itm, steps.get(itm)+1);
}

function destroy(itm) {
	stars = stars.filter (function (f) { return f !== itm })
	mx_steps.delete (itm);
	inleft.delete (itm);
	steps.delete (itm);
	angle.delete (itm);
	x_y.delete (itm);
	velocity.delete (itm);
	faded.delete (itm);
	fadedSteps.delete (itm);
	let el = document.getElementById (itm);
	document.getElementById ('star_container').removeChild (el);
}


let start = Date.now ();
const refreshRate = 40;
const stepsToHide = 300 / refreshRate;

document.addEventListener ("mousemove", function (e) {
	const millis = Date.now () - start;
	if (millis >= refreshRate) {
		start = Date.now ();
		const p1 = {
			x: e.clientX,
			y: e.clientY
		};
		const delta = Math.abs (p1.x - lastPosX) + Math.abs (p1.y - lastPosY);
		let canAdd = true;
		if (delta < 10) {
			canAdd = millis >= refreshRate * 2;
		}
		if (canAdd) {
			const angleDeg = Math.atan2 (lastPosY - p1.y, lastPosX - p1.x) * 180 / Math.PI;
			lastPosX = e.clientX;
			lastPosY = e.clientY;
			add (p1, Math.round (angleDeg), 90, true, max_steps, 4);
		}
	}
});

function Click(e) {
	e = e || window.event;
	const p1 = {
		x: e.clientX,
		y: e.clientY
	};
	const angleDeg = Math.atan2 (lastPosY - p1.y, lastPosX - p1.x) * 180 / Math.PI;
	for (let i = 1; i < 25; i++) {
		let spd = (Math.random () + 0.5) * 4;
		add (p1, Math.round (angleDeg), 15 * i, true, 30, spd);
	}

}

function updateStars() {
	setTimeout (function () {
		if (stars.length > 0) {
			for (let index = 0; index < stars.length; ++index) {
				const star = stars[index];
				update (star);
			}
		}
		updateStars ();
	}, refreshRate);
}

updateStars ();

