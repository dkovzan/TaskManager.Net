window.onload = function () {
    var project = JSON.parse(localStorage.getItem("project"));
    if (project !== null && project !== undefined) {
        document.getElementById("Name").value = project.name;
        document.getElementById("ShortName").value = project.shortName;
        document.getElementById("Description").value = project.description;
    }
}

function saveTempProject() {
    var name = document.getElementById("Name").value;
    var shortName = document.getElementById("ShortName").value;
    var description = document.getElementById("Description").value;
    var project = { "name": name, "shortName": shortName, "description": description };
    localStorage.setItem("project", JSON.stringify(project));
}