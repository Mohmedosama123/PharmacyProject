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

    // Medication details modal functionality
    const viewDetailsButtons = document.querySelectorAll('.view-details-btn');
    const medicationDetailsModal = document.getElementById('medicationDetailsModal');
    const modalMedicationName = document.getElementById('modal-medication-name');
    const modalMedicationCategory = document.getElementById('modal-medication-category');
    const modalMedicationPrice = document.getElementById('modal-medication-price');
    const modalMedicationDescription = document.getElementById('modal-medication-description');
    const modalAddMedicationBtn = document.getElementById('modal-add-medication-btn');

    // Mock medication data for demo purposes
    const medicationDetails = {
        '1': {
            name: 'Paracetamol',
            category: 'Analgesic & Antipyretic',
            price: '15.50 EGP',
            description: 'Paracetamol is a common pain reliever and fever reducer. It is used to treat many conditions such as headache, muscle aches, arthritis, backache, toothaches, colds, and fevers.'
        },
        '2': {
            name: 'Amoxicillin',
            category: 'Antibiotic',
            price: '45.75 EGP',
            description: 'Amoxicillin is a penicillin antibiotic that fights bacteria. It is used to treat many different types of infection caused by bacteria, such as tonsillitis, bronchitis, pneumonia, and infections of the ear, nose, throat, skin, or urinary tract.'
        },
        '3': {
            name: 'Omeprazole',
            category: 'Proton Pump Inhibitor',
            price: '55.50 EGP',
            description: 'Omeprazole is used to treat certain stomach and esophagus problems (such as acid reflux, ulcers). It works by decreasing the amount of acid your stomach makes.'
        },
        '4': {
            name: 'Ibuprofen',
            category: 'NSAID',
            price: '20.25 EGP',
            description: 'Ibuprofen is a nonsteroidal anti-inflammatory drug (NSAID). It works by reducing hormones that cause inflammation and pain in the body.'
        },
        '5': {
            name: 'Loratadine',
            category: 'Antihistamine',
            price: '35.00 EGP',
            description: 'Loratadine is an antihistamine that reduces the effects of natural chemical histamine in the body. Histamine can produce symptoms of sneezing, itching, watery eyes, and runny nose.'
        },
        '6': {
            name: 'Metformin',
            category: 'Antidiabetic',
            price: '42.50 EGP',
            description: 'Metformin is an oral diabetes medicine that helps control blood sugar levels. It is used together with diet and exercise to improve blood sugar control in adults with type 2 diabetes.'
        },
        '7': {
            name: 'Atorvastatin',
            category: 'Statin',
            price: '65.75 EGP',
            description: 'Atorvastatin is in a group of drugs called HMG CoA reductase inhibitors, or "statins." It reduces levels of "bad" cholesterol (low-density lipoprotein, or LDL) and triglycerides in the blood, while increasing levels of "good" cholesterol (high-density lipoprotein, or HDL).'
        },
        '8': {
            name: 'Salbutamol',
            category: 'Bronchodilator',
            price: '38.25 EGP',
            description: 'Salbutamol is a bronchodilator that relaxes muscles in the airways and increases air flow to the lungs. It is used to treat or prevent bronchospasm in people with reversible obstructive airway disease.'
        }
    };

    if (viewDetailsButtons.length > 0 && medicationDetailsModal) {
        viewDetailsButtons.forEach(button => {
            button.addEventListener('click', function () {
                const medicationId = this.getAttribute('data-medication-id');
                const details = medicationDetails[medicationId];

                if (details) {
                    modalMedicationName.textContent = details.name;
                    modalMedicationCategory.textContent = details.category;
                    modalMedicationPrice.textContent = details.price;
                    modalMedicationDescription.textContent = details.description;

                    // Set the add button data attributes
                    modalAddMedicationBtn.setAttribute('data-medication-id', medicationId);
                    modalAddMedicationBtn.setAttribute('data-medication-name', details.name);
                    modalAddMedicationBtn.setAttribute('data-medication-price', details.price.replace(' EGP', ''));

                    // Show the modal
                    const modal = new bootstrap.Modal(medicationDetailsModal);
                    modal.show();
                }
            });
        });

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