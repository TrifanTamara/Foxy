jQuery(document).ready(function () {
    $('.my-check-icon').each(function () {
        this.style.display = "none";
    });
});

function isHidden(el) {
    var style = window.getComputedStyle(el);
    return (style.display === 'none')
}

function CheckBox(qi, ai) {
    var elementBox = document.getElementById("icon" + qi + "-" + ai);
    if (isHidden(elementBox)) {
        elementBox.style.display = "inline-block";
    } else {
        elementBox.style.display = "none";
    }
}
