document.addEventListener("DOMContentLoaded", function () {
    var checkboxDevolvidoElements = document.querySelectorAll(".checkboxDevolvido");

    checkboxDevolvidoElements.forEach(function (checkbox) {
        checkbox.addEventListener("change", function () {
            const isChecked = checkbox.checked;
            AtualizaDevolvidoStatusUI(checkbox, isChecked);

            localStorage.setItem("devolvidoCheckbox_" + checkbox.dataset.emprestimoId, isChecked.toString());

            var event = new CustomEvent("devolvidoStatusChanged", {
                detail: { emprestimoId: checkbox.dataset.emprestimoId, isChecked: isChecked }
            });
            document.dispatchEvent(event);
        });
        
        AtualizaDevolvidoStatusUI(checkbox);
    });

    function AtualizaDevolvidoStatusUI(checkbox, isChecked) {
        var emprestimoId = checkbox.dataset.emprestimoId;
        var devolvidoLabelElement = document.querySelector(".devolvido-label[data-emprestimo-id='" + emprestimoId + "']");

        if (devolvidoLabelElement) {
            
            var storedState = localStorage.getItem("devolvidoCheckbox_" + emprestimoId);
            if (storedState !== null) {
                isChecked = storedState === "true";
                checkbox.checked = isChecked; 
            }

            devolvidoLabelElement.textContent = isChecked ? "Devolvido" : "Não Devolvido";
            devolvidoLabelElement.style.color = isChecked ? "green" : "red";
        }
    }
});