function showSearchDialog() {
    document.getElementById("search-copy-btn").click();
    closeNav();
}



function searchCopy() {
    var id = parseInt(document.getElementById("copy-id").value);
    if (isNaN(id)) {
        alert("Id is not valid!");
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

function addLoanType() {

    var isValid = true;

    var typeName = document.getElementById("type-name");
    var numLoans = document.getElementById("num-loans");

    var typeNameVal = typeName.value;
    var numLoansVal = parseInt(numLoans.value);

    // Check if the input string is null or whitespace
    if (!(typeNameVal && typeNameVal.trim())) {
        isValid = false;
        alert("Please add a type name.")
    }

    if (isNaN(numLoansVal) || numLoansVal < 1) {
        isValid = false;
        alert("Please add a valid number of days.");
    }

    if (isValid) {
        var mData = JSON.stringify({ "LoanTypeId": 0, "LoanTypeName": typeNameVal, "DurationDays": numLoansVal });


        const settings = {
            "async": true,
            "crossDomain": true,
            "url": "/api/loantype",
            "method": "POST",
            "headers": {
                "content-type": "application/json"
            },
            "processData": false,
            "data": mData
        };

        $.ajax(settings).done(function (response) {
            alert("Loan type added!")
            window.location.reload(true);
        });
    }

}