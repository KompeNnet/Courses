$(function () {

    var drawGame = {
        isDrawing: false,
        startX: 0,
        startY: 0,
    };

    var data = {
        startX: 0,
        startY: 0,
        endX: 0,
        endY: 0
    };

    var canvas = document.getElementById('drawingpad');
    var ctx = canvas.getContext('2d');

    var chat = $.connection.drawHub;

    chat.client.addLine = function (data) {
        drawLine(ctx, data.startX, data.startY, data.endX, data.endY, 1);
    };

    $.connection.hub.start().done(function () {
        canvas.addEventListener("mousedown", mousedown, false);
        canvas.addEventListener("mousemove", mousemove, false);
        canvas.addEventListener("mouseup", mouseup, false);
    });

    function drawLine(ctx, x1, y1, x2, y2, thickness) {
        ctx.beginPath();
        ctx.moveTo(x1, y1);
        ctx.lineTo(x2, y2);
        ctx.lineWidth = thickness;
        ctx.strokeStyle = "#444";
        ctx.stroke();
    }

    function mousedown(e) {
        var mouseX = e.layerX || 0;
        var mouseY = e.layerY || 0;
        drawGame.startX = mouseX;
        drawGame.startY = mouseY;
        drawGame.isDrawing = true;
    };
    
    function mousemove(e) {
        if (drawGame.isDrawing) {
            var mouseX = e.layerX || 0;
            var mouseY = e.layerY || 0;
            if (!(mouseX == drawGame.startX && mouseY == drawGame.startY)) {
                drawLine(ctx, drawGame.startX, drawGame.startY, mouseX, mouseY, 1);

                data.startX = drawGame.startX;
                data.startY = drawGame.startY;
                data.endX = mouseX;
                data.endY = mouseY;
                chat.server.send(data);

                drawGame.startX = mouseX;
                drawGame.startY = mouseY;
            }
        }
    };

    function mouseup(e) {
        drawGame.isDrawing = false;
    }
});