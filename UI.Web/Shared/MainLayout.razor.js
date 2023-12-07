
export function AddThemeHandlerAndGetCurrentTheme() {
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', event => {
        const isDark = event.matches;
        HandleThemeValue(isDark);
    });

    const currentIsDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    HandleThemeValue(currentIsDark);
}

function HandleThemeValue(isDark) {
    var root = document.querySelector(':root');
    root.style.setProperty('--todoos-bg-color-l', isDark ? '10%' : '90%');
    /*let formData = new FormData();
    formData.append('isDark', isDark);

    fetch("Api/Theme/Set", {
        method: "POST",
        body: formData
    });  */
}