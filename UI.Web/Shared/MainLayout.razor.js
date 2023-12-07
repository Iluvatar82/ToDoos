
export function AddThemeHandlerAndGetCurrentTheme() {
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', event => {
        const isDark = event.matches;
        SendThemeValue(isDark);
    });

    const currentIsDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    SendThemeValue(currentIsDark);
}

function SendThemeValue(isDark) {
    let formData = new FormData();
    formData.append('isDark', isDark);

    fetch("Api/Theme/Set", {
        method: "POST",
        body: formData
    });
}