function showSearchDialog() {
    document.getElementById("search-copy-btn").click();
    closeNav();
}

function searchCopy() {
    var id = parseInt(document.getElementById("copy-id").value);
    if (isNaN(id)) {
        alert("You moron!");
    }
    else {
        var url = "/Dvdcopies/Details/".concat(id.toString());

        // Check if a copy with given id exists.
        var http = new XMLHttpRequest();
        http.open('HEAD', url, false);
        http.send();
        if (http.status == 404)
            alert("Copy with given id not found!");
        else
            window.location.href = "/Dvdcopies/Details/".concat(id.toString());

    }
}