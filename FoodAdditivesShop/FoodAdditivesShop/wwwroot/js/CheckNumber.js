function CheckNumber(e) {
    e.preventDefault()

    let count = document.getElementById("count").value;

    if (Number(count < 1)) {
        alert("Трябва да добавите поне 1 бройка!")
        return false;
    }

    document.getElementById("myForm").submit();
}