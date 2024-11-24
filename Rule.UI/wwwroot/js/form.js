document.addEventListener('DOMContentLoaded', function () {
    const category = document.getElementById("category").value;
    const amountField = document.getElementById("amountField");

    
    if (category === "zbir") {
        amountField.style.display = "block";
    } else {
        amountField.style.display = "none";
    }

    document.getElementById("category").addEventListener("change", function() {
        if (this.value === "gathering") {
            amountField.style.display = "block";
        } else {
            amountField.style.display = "none";
        }
    });
});
