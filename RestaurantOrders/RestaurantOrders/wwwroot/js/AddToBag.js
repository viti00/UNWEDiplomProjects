function AddToBag(id) {

    var inputEl = document.getElementById(id);

    $.ajax({
        url: `/Bags/Details?handler=AddToBag&pId=${id}&quantity=${inputEl.value}`,
        success: function (data) {
            alert(data);
            inputEl.value = "";
            GetCount();
        },
        error: function (error) {
            
        }
    })
}