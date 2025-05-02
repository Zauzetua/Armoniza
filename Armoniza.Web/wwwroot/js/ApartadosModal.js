(() => {
    let currentSlot = null;
    let isModalClosing = false;
    const modalEl = document.getElementById('instrumentModal');
    const modal = new bootstrap.Modal(modalEl);
    const instrumentosSeleccionados = new Set();  // Para llevar control de los instrumentos seleccionados

    // Limpia la bandera al cerrar el modal
    modalEl.addEventListener('hidden.bs.modal', () => {
        isModalClosing = false;
    });

    // Abrir modal al hacer clic en cualquier slot
    document.querySelectorAll('.slot-btn').forEach(btn => {
        btn.addEventListener('click', e => {
            if (isModalClosing) return;
            currentSlot = e.currentTarget.dataset.index;
            modal.show();

            // Recuperar los instrumentos seleccionados
            const selectedInstrumentIds = Array.from(instrumentosSeleccionados);

            // Ocultar los instrumentos seleccionados en el modal
            modal.querySelectorAll('.inst-item').forEach(item => {
                const itemId = item.dataset.id;
                if (selectedInstrumentIds.includes(itemId)) {
                    item.classList.add('d-none');  // Ocultar el instrumento si ya está seleccionado
                } else {
                    item.classList.remove('d-none');  // Asegurarse de que otros no estén ocultos
                }
            });
        });
    });

    // Al hacer clic en "Seleccionar" dentro del modal
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

            // Agregar el instrumento a la lista de seleccionados
            instrumentosSeleccionados.add(id);

            // Ocultar el instrumento seleccionado
            const listItem = e.currentTarget.closest('.inst-item');
            if (listItem) listItem.classList.add('d-none');

            // Cerrar el modal correctamente
            modal.hide();
        });
    });

    // Limpiar slot (desmarcar instrumento)
    document.querySelectorAll('.clear-slot-btn').forEach(btn => {
        btn.addEventListener('click', e => {
            const index = e.currentTarget.dataset.index;
            const input = document.getElementById(`slot-input-${index}`);
            const oldId = input.value;

            // Limpiar el valor del slot
            input.value = 0;
            document.getElementById(`slot-name-${index}`).innerHTML = "<em>Ninguno</em>";

            const slotBtn = document.querySelector(`.slot-btn[data-index="${index}"]`);
            slotBtn.classList.remove('btn-outline-success');
            slotBtn.classList.add('btn-outline-primary');
            slotBtn.innerHTML = '<i class="bi bi-plus-lg"></i>';

            // Eliminar de los seleccionados
            instrumentosSeleccionados.delete(oldId);

            // Volver a mostrar el instrumento deseleccionado en el modal
            const itemToShow = document.querySelector(`.inst-item button[data-inst-id="${oldId}"]`)?.closest('.inst-item');
            if (itemToShow) itemToShow.classList.remove('d-none');
        });
    });

    // Search functionality in the modal
    const searchInput = modalEl.querySelector('#instrumentSearch');
    if (searchInput) {
        searchInput.addEventListener('input', () => {
            const term = searchInput.value.trim().toLowerCase();
            const items = modalEl.querySelectorAll('.inst-item');

            items.forEach(item => {
                const nombre = item.getAttribute('data-nombre') || '';
                const codigo = item.getAttribute('data-codigo') || '';
                const matches = (nombre.includes(term) || codigo.includes(term));

                // Check if the item is already selected
                const isSelected = instrumentosSeleccionados.has(item.dataset.id);

                // Show or hide items based on the search term and selection status
                if (!isSelected) {
                    item.classList.toggle('d-none', !matches);
                }
            });

            // If the search term is empty, show all non-selected items
            if (term === "") {
                items.forEach(item => {
                    const isSelected = instrumentosSeleccionados.has(item.dataset.id);
                    if (!isSelected) {
                        item.classList.remove('d-none');
                    }
                });
            }
        });
    }
})();
