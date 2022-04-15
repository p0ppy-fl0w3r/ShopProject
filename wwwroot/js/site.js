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