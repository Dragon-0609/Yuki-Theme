let cursor = document.getElementById("cursor");

let setCursorPosition = function(e) {
  let xPosition = e.clientX - cursor.clientWidth / 2 + "px";
  let yPosition = e.clientY - cursor.clientHeight / 2 + "px";
  cursor.style.transform =
    "translate(" + xPosition + "," + yPosition + ") scale(1)";
  return {
    x: xPosition,
    y: yPosition
  };
};

document.addEventListener("mousemove", e => setCursorPosition(e));
let scaleCursor = function(e, scale) {
  let ps = setCursorPosition(e);
  cursor.style.transform =
    "translate(" +
    ps.x +
    "," +
    ps.y +
    ") scale(" +
    scale +
    ")";
};

document.addEventListener("mousedown", function(e) {
  scaleCursor(e, 0.6);
});
document.addEventListener("mouseup", function(e) {
  scaleCursor(e, 1);
});