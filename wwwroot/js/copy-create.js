function onSubmit() {
    var submitButton = document.getElementById("submit-copy");
    submitButton.addEventListener("click", checkAndSubmit);
}

function checkAndSubmit() {

    var dvdId = parseInt(document.getElementById("dvd-id-select").value);
    var count = parseInt(document.getElementById("count-field").value);
    var dateVal = document.getElementById("date-field").valueAsDate;

    var movieError = document.getElementById("movie-error");
    var dateError = document.getElementById("date-error");
    var countError = document.getElementById("count-error");



    var dateStr = "";

    var isValid = true;

    if (isNaN(dvdId)) {
        isValid = false;
        movieError.style.visibility = "visible";
    }
    else {
        movieError.style.visibility = "hidden";
    }


    if (dateVal == null) {
        isValid = false;
        dateError.style.visibility = "visible";
    }
    else {
        dateStr = dateVal.toISOString();
        dateError.style.visibility = "hidden";
    }

    if (isNaN(count)) {
        countError.style.visibility = "visible";
    }
    else {
        if (count < 1 || count > 20) {
            isValid = false;
            countError.style.visibility = "visible";
        } else {
            countError.style.visibility = "hidden";
        }
    }

    if (isValid) {
        var mData = JSON.stringify({ "CopyId": 0, "DvdId": dvdId, "DatePurchased": dateStr, "TotalCopies": count });


        const settings = {
            "async": true,
            "crossDomain": true,
            "url": "/api/copies",
            "method": "POST",
            "headers": {
                "content-type": "application/json"
            },
            "processData": false,
            "data": mData
        };

        $.ajax(settings).done(function (response) {
            window.location.reload(true);
        });
    }
}

onSubmit();