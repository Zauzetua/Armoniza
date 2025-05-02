(() => {
    let currentSlot = null;
    let isModalClosing = false;

    const modalEl = document.getElementById('instrumentModal');
    const modal = new bootstrap.Modal(modalEl);

    // Limpia la bandera al cerrar
    modalEl.addEventListener('hidden.bs.modal', () => {
        isModalClosing = false;
    });

    // Abrir modal al hacer clic en cualquier slot
    document.querySelectorAll('.slot-btn').forEach(btn => {
        btn.addEventListener('click', e => {
            if (isModalClosing) return;
            currentSlot = e.currentTarget.dataset.index;
            modal.show();
        });
    });

    // Al hacer clic en “Seleccionar” dentro del modal
    document.querySelectorAll('.select-inst-btn').forEach(btn => {
        btn.addEventListener('click', e => {
            if (isModalClosing) return;
            isModalClosing = true;

            const id = e.currentTarget.dataset.instId;
            const name = e.currentTarget.dataset.instName;

            // Setear valor del slot
            document.getElementById(`slot-input-${currentSlot}`).value = id;
            document.getElementById(`slot-name-${currentSlot}`).innerText = name;

            const slotBtn = document.querySelector(`.slot-btn[data-index="${currentSlot}"]`);
            slotBtn.classList.replace('btn-outline-primary', 'btn-outline-success');
            slotBtn.innerHTML = '<i class="bi bi-pencil"></i>';

            // Cerrar modal correctamente
            modal.hide();
        });
    });
})();

document.querySelectorAll('.clear-slot-btn').forEach(btn => {
    btn.addEventListener('click', e => {
        const index = e.currentTarget.dataset.index;
        document.getElementById(`slot-input-${index}`).value = 0;
        document.getElementById(`slot-name-${index}`).innerHTML = "<em>Ninguno</em>";

        const slotBtn = document.querySelector(`.slot-btn[data-index="${index}"]`);
        slotBtn.classList.remove('btn-outline-success');
        slotBtn.classList.add('btn-outline-primary');
        slotBtn.innerHTML = '<i class="bi bi-plus-lg"></i>';
    });
});