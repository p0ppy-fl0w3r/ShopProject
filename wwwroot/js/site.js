// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function toggleDisable(mToggleSwitch, mField) {
    mToggleSwitch.addEventListener("change", function (event) {
        if (mToggleSwitch.checked) {
            mField.disabled = true;
        }
        else {
            mField.disabled = false;
        }
    });
}


function openNav() {
    document.getElementById("mySidenav").style.width = "300px";
    document.getElementById("mySidenav").style.left = "0";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("mySidenav").style.left = "-100px";
    document.getElementById("container").style.marginLeft = "auto";
    document.body.style.backgroundColor = "white";
}

function eraseSideLogo() {
    if ("/" != location.pathname) {
        document.getElementById("nav-icon").style.fontSize = "0";

    }
    else {
        document.getElementById("nav-icon").style.fontSize = "30px";
        
    }

}

eraseSideLogo();