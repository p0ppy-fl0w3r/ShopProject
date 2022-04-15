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

var categorySelect = document.getElementById("category-select");
var categorySwitch = document.getElementById("new-category-toggle");

var producerSelect = document.getElementById("producer-select");
var producerSwitch = document.getElementById("new-producer-toggle");

var studioSelect = document.getElementById("studio-select");
var studioSwitch = document.getElementById("new-studio-toggle");

toggleDisable(categorySwitch, categorySelect);
toggleDisable(producerSwitch, producerSelect);
toggleDisable(studioSwitch, studioSelect);

$.validator.setDefaults({ ignore: null });


$("#submit").click(function (e) {

    if (checkPrivRows()) {
        var actorIds = [];

        $("select[name='ActorId']").each(function (i) {
            var selecteId = parseInt($(this).val());
            actorIds.push(selecteId);
        });

        // Convert to base64
        var actorValue = btoa(JSON.stringify(actorIds));

        document.cookie = "actorIds=".concat(actorValue).concat(";");
    }
    else {
        e.preventDefault();
        alert("Some of the fields are not valid!");
    }

});



$("#addActor").click(function () {
    if (checkPrivRows()) {

        document.getElementById("hasActor").textContent = "1";

        $.ajax({
            url: "/Catalog/AddActorItem",
            cache: false,
            success: function (html) { $("#actor-rows").append(html); }
        });
        return false;
    }
    else {
        document.getElementById("hasActor").textContent = "0";
        alert("Looks like one of the actor fields is empty!");
    }
});

function checkPrivRows() {
    var isOk = true;
    $("select[name='ActorId']").each(function (i) {
        var selecteId = parseInt($(this).val());
        if (isNaN(selecteId)) {
            isOk = false;
            this.style.color = "red";
        }
        else {
            this.style.color = "black";
        }
    });

    return isOk;
}


$(document.body).on('click', 'a.deleteRow', function () {
    $(this).closest("div.actor-row").remove();
});