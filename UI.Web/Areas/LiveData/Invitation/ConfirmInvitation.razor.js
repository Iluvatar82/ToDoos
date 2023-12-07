export function SendLoginRequest(email, password, rememberme)
{
    let formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);
    formData.append('rememberme', rememberme);

    fetch("Identity/Account/Login?handler=Invitation", {
        method: "POST",
        body: formData
    })
    .then((response) => { window.location.href = "/"; });
}
