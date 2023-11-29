
var topLevelSelector = 'table tbody tr.top';
var currentElement = null;
var currentIndex = 0;
var allowKeyboardNavigation = true;


function onclick(e) {
    if (currentElement == null)
        return;

    currentIndex = 0;
    currentElement.classList.remove('active');
    currentElement = null;
};

function onkeyup(e) {
    var isNewItemAdding = document.querySelectorAll('.newItem').length > 0;
    if (e.key == "j" && allowKeyboardNavigation && !isNewItemAdding) {
        let topLevelElements = document.querySelectorAll(topLevelSelector);

        if (currentElement != null)
            currentIndex++;

        if (currentIndex >= topLevelElements.length)
            currentIndex = 0;

        if (currentElement != null)
            currentElement.classList.remove('active');

        currentElement = topLevelElements[currentIndex];
        currentElement.classList.add('active');

        window.scrollTo({
            behavior: 'smooth',
            top:
                currentElement.getBoundingClientRect().top -
                document.body.getBoundingClientRect().top -
                document.querySelectorAll('.top-row')[0].getBoundingClientRect().height,
        });
    }
    else if (e.key == "k" && allowKeyboardNavigation && !isNewItemAdding) {
        let topLevelElements = document.querySelectorAll(topLevelSelector);

        if (currentElement != null)
            currentIndex--;

        if (currentIndex < 0)
            currentIndex = topLevelElements.length - 1;

        if (currentElement != null)
            currentElement.classList.remove('active');

        currentElement = topLevelElements[currentIndex];
        currentElement.classList.add('active');

        window.scrollTo({
            behavior: 'smooth',
            top:
                currentElement.getBoundingClientRect().top -
                document.body.getBoundingClientRect().top -
                document.querySelectorAll('.top-row')[0].getBoundingClientRect().height,
        });
    }
};

export function allowKeyNavigation(value) {
    allowKeyboardNavigation = value;
    if (value == false && currentElement != null) {
        currentElement.classList.remove('active');
        currentElement = null;
    }
};

export function initializeScrollHandler() {
    addEventListener('keyup', (event) => onkeyup(event));
    addEventListener('click', (event) => onclick(event));
}