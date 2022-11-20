var UnoAppManifest = {

    splashScreenImage: "Assets/opensenselogo.png",
    splashScreenColor: "transparent",
    displayName: "timeboxed"

}

window.SendNotification = (title, msg) => new Notification(title, { body: msg, icon: UnoAppManifest.splashScreenImage });

window.onload = () => {
    if(Notification.permission != "granted")
        Notification.requestPermission();
}