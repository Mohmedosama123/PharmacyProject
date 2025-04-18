document.getElementById('current-year').textContent = new Date().getFullYear();

// Add Medication Catalog Functionality
document.addEventListener('DOMContentLoaded', function () {
    // View toggle functionality
    const gridViewBtn = document.getElementById('grid-view-btn');
    const listViewBtn = document.getElementById('list-view-btn');
    const gridView = document.getElementById('medications-grid-view');
    const listView = document.getElementById('medications-list-view');

    if (gridViewBtn && listViewBtn) {
        gridViewBtn.addEventListener('click', function () {
            gridView.classList.remove('d-none');
            listView.classList.add('d-none');
            gridViewBtn.classList.add('active');
            listViewBtn.classList.remove('active');
        });

        listViewBtn.addEventListener('click', function () {
            gridView.classList.add('d-none');
            listView.classList.remove('d-none');
            gridViewBtn.classList.remove('active');
            listViewBtn.classList.add('active');
        });
    }

    // Medication search functionality
    const medicationCatalogSearch = document.getElementById('medicationCatalogSearch');
    const medicationItems = document.querySelectorAll('.medication-catalog-item');

    if (medicationCatalogSearch && medicationItems.length > 0) {
        medicationCatalogSearch.addEventListener('input', function () {
            const searchTerm = this.value.toLowerCase();

            medicationItems.forEach(item => {
                const medicationName = item.querySelector('.card-title, .fw-medium').textContent.toLowerCase();
                if (medicationName.includes(searchTerm)) {
                    item.style.display = '';
                } else {
                    item.style.display = 'none';
                }
            });
        });
    }

    // Add medication functionality
    const addButtons = document.querySelectorAll('.add-medication-btn');
    const selectedMedicationsList = document.getElementById('selected-medications-list');
    const noMedicationsSelected = document.getElementById('no-medications-selected');
    const selectedMedicationsTableContainer = document.getElementById('selected-medications-table-container');
    const clearAllBtn = document.getElementById('clear-all-btn');
    const saveAllMedicationsBtn = document.getElementById('save-all-medications-btn');

    let selectedMedications = [];

    if (addButtons.length > 0 && selectedMedicationsList) {
        addButtons.forEach(button => {
            button.addEventListener('click', function () {
                const medicationId = this.getAttribute('data-medication-id');
                const medicationName = this.getAttribute('data-medication-name');
                const medicationPrice = this.getAttribute('data-medication-price');

                // Check if medication is already in the list
                if (!selectedMedications.some(med => med.id === medicationId)) {
                    selectedMedications.push({
                        id: medicationId,
                        name: medicationName,
                        price: medicationPrice,
                        quantity: 1,
                        inStock: true
                    });

                    updateSelectedMedicationsList();

                    // Show success toast or notification
                    alert(`${medicationName} added to your inventory!`);
                } else {
                    alert(`${medicationName} is already in your inventory!`);
                }
            });
        });

        // Clear all selected medications
        if (clearAllBtn) {
            clearAllBtn.addEventListener('click', function () {
                if (confirm('Are you sure you want to clear all selected medications?')) {
                    selectedMedications = [];
                    updateSelectedMedicationsList();
                }
            });
        }

        // Save all medications
        if (saveAllMedicationsBtn) {
            saveAllMedicationsBtn.addEventListener('click', function () {
                if (selectedMedications.length > 0) {
                    alert('All medications have been saved to your inventory!');
                    // Here you would typically send the data to the server
                    // For demo purposes, we'll just redirect to the medications tab
                    document.getElementById('medications-tab').click();
                } else {
                    alert('Please select at least one medication to save.');
                }
            });
        }
    }

    // Function to update the selected medications list
    function updateSelectedMedicationsList() {
        if (selectedMedicationsList) {
            // Clear the current list
            selectedMedicationsList.innerHTML = '';

            if (selectedMedications.length > 0) {
                // Hide the "no medications selected" message
                if (noMedicationsSelected) noMedicationsSelected.classList.add('d-none');
                if (selectedMedicationsTableContainer) selectedMedicationsTableContainer.classList.remove('d-none');

                // Add each medication to the list
                selectedMedications.forEach(medication => {
                    const row = document.createElement('tr');
                    row.innerHTML = `
                <td>${medication.name}</td>
                <td>${medication.price}</td>
                <td>
                  <input type="number" class="form-control form-control-sm" value="${medication.quantity}" min="1" data-medication-id="${medication.id}">
                </td>
                <td>
                  <div class="form-check">
                    <input class="form-check-input" type="checkbox" ${medication.inStock ? 'checked' : ''} data-medication-id="${medication.id}">
                  </div>
                </td>
                <td class="text-end">
                  <button type="button" class="btn btn-sm btn-outline-danger remove-medication-btn" data-medication-id="${medication.id}">
                    <i class="bi bi-trash"></i>
                  </button>
                </td>
              `;
                    selectedMedicationsList.appendChild(row);

                    // Add event listener to the remove button
                    const removeButton = row.querySelector('.remove-medication-btn');
                    removeButton.addEventListener('click', function () {
                        const medicationId = this.getAttribute('data-medication-id');
                        selectedMedications = selectedMedications.filter(med => med.id !== medicationId);
                        updateSelectedMedicationsList();
                    });

                    // Add event listeners to quantity input and in-stock checkbox
                    const quantityInput = row.querySelector('input[type="number"]');
                    quantityInput.addEventListener('change', function () {
                        const medicationId = this.getAttribute('data-medication-id');
                        const medication = selectedMedications.find(med => med.id === medicationId);
                        if (medication) {
                            medication.quantity = parseInt(this.value) || 1;
                        }
                    });

                    const inStockCheckbox = row.querySelector('input[type="checkbox"]');
                    inStockCheckbox.addEventListener('change', function () {
                        const medicationId = this.getAttribute('data-medication-id');
                        const medication = selectedMedications.find(med => med.id === medicationId);
                        if (medication) {
                            medication.inStock = this.checked;
                        }
                    });
                });
            } else {
                // Show the "no medications selected" message
                if (noMedicationsSelected) noMedicationsSelected.classList.remove('d-none');
                if (selectedMedicationsTableContainer) selectedMedicationsTableContainer.classList.add('d-none');
            }
        }
    }



    //-------------------------------------------

    button.addEventListener('click', function () {
    const medicationId = this.getAttribute('data-medication-id');
    const name = this.getAttribute('data-name');
    const category = this.getAttribute('data-category');
    const price = this.getAttribute('data-price');
    const description = this.getAttribute('data-description') || "No description available";
    const imageUrl = this.getAttribute('data-image-url');

    modalMedicationName.textContent = name;
    modalMedicationCategory.textContent = category;
    modalMedicationPrice.textContent = price;
    modalMedicationDescription.textContent = description;

    if (imageUrl) {
        modalMedicationImage.setAttribute('src', imageUrl);
        modalMedicationImage.setAttribute('alt', name);
    }

    modalAddMedicationBtn.setAttribute('data-medication-id', medicationId);
    modalAddMedicationBtn.setAttribute('data-medication-name', name);
    modalAddMedicationBtn.setAttribute('data-medication-price', price.replace(' EGP', ''));

    const modal = new bootstrap.Modal(medicationDetailsModal);
    modal.show();
});

//------------------------------------------------------------------------------


        // Add medication from modal
        if (modalAddMedicationBtn) {
            modalAddMedicationBtn.addEventListener('click', function () {
                const medicationId = this.getAttribute('data-medication-id');
                const medicationName = this.getAttribute('data-medication-name');
                const medicationPrice = this.getAttribute('data-medication-price');

                // Check if medication is already in the list
                if (!selectedMedications.some(med => med.id === medicationId)) {
                    selectedMedications.push({
                        id: medicationId,
                        name: medicationName,
                        price: medicationPrice,
                        quantity: 1,
                        inStock: true
                    });

                    updateSelectedMedicationsList();

                    // Show success toast or notification
                    alert(`${medicationName} added to your inventory!`);

                    // Close the modal
                    const modal = bootstrap.Modal.getInstance(medicationDetailsModal);
                    modal.hide();
                } else {
                    alert(`${medicationName} is already in your inventory!`);
                }
            });
        }
    }
});

////// ===================================
//let selectedMedications = [];

//document.addEventListener("DOMContentLoaded", function () {
//    // عند الضغط على زر الإضافة (+) لكل دواء
//    document.querySelectorAll(".add-medication-btn").forEach(button => {
//        button.addEventListener("click", function () {
//            let medicationId = this.dataset.medicationId;
//            if (!selectedMedications.includes(medicationId)) {
//                selectedMedications.push(medicationId);
//                alert("Medication added!"); // تأكيد الإضافة
//            }
//        });
//    });

//    // عند الضغط على زر "Save All Medications"
//    document.getElementById("save-medications-btn").addEventListener("click", function () {
//        if (selectedMedications.length === 0) {
//            alert("No medications selected!");
//            return;
//        }

//        fetch("/Pharmacy/Dashbord", {
//            method: "POST",
//            headers: {
//                "Content-Type": "application/json"
//            },
//            body: JSON.stringify({ medicationIds: selectedMedications })
//        }).then(response => {
//            if (response.ok) {
//                alert("All medications added successfully!");
//                selectedMedications = []; // تفريغ المصفوفة بعد الحفظ
//            } else {
//                alert("Failed to add medications.");
//            }
//        });
//    });
//});