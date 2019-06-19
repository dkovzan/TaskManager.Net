window.onload = function () {
    var project = JSON.parse(localStorage.getItem("project"));
    document.getElementById("ProjectShortName").value = project.shortName;
}
