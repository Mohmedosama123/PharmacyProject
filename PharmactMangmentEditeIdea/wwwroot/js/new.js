document.addEventListener('DOMContentLoaded', function () {
    // Store selected medication IDs
    let selectedMedications = [];

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
    const saveMedicationsBtn = document.getElementById('save-medications-btn');

    if (addButtons.length > 0 && selectedMedicationsList) {
        addButtons.forEach(button => {
            button.addEventListener('click', function () {
                const medicationId = parseInt(this.getAttribute('data-medication-id'));
                const medicationName = this.getAttribute('data-medication-name');
                const medicationPrice = this.getAttribute('data-medication-price');

                // Check if medication is already in the array
                if (!selectedMedications.some(id => id === medicationId)) {
                    // Add to the array
                    selectedMedications.push(medicationId);

                    // Update the UI
                    updateSelectedMedicationsList();

                    // Show feedback
                    alert(`${medicationName} added to your selection!`);
                } else {
                    alert(`${medicationName} is already in your selection!`);
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
        if (saveMedicationsBtn) {
            saveMedicationsBtn.addEventListener('click', function () {
                if (selectedMedications.length === 0) {
                    alert('Please select at least one medication to save.');
                    return;
                }

                console.log('Selected medications:', selectedMedications);

                // Show loading state
                this.disabled = true;
                this.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Saving...';

                // Get the anti-forgery token
                const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
                console.log('Anti-forgery token:', token ? 'Found' : 'Not found');

                // Try the fetch approach first
                try {
                    // Send the data to the server
                    fetch(window.location.href, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': token
                        },
                        body: JSON.stringify(selectedMedications)
                    })
                        .then(response => {
                            console.log('Response status:', response.status);
                            if (!response.ok) {
                                throw new Error(`HTTP error! Status: ${response.status}`);
                            }
                            return response.json();
                        })
                        .then(data => {
                            console.log('Response data:', data);
                            if (data.success) {
                                alert(data.message);
                                // Clear the selection
                                selectedMedications = [];
                                updateSelectedMedicationsList();

                                // Optional: Redirect to another tab
                                document.getElementById('medications-tab').click();
                            } else {
                                alert('Error: ' + data.message);
                            }
                        })
                        .catch(error => {
                            console.error('Error:', error);
                            // If fetch fails, fall back to form submission
                            submitFormFallback();
                        })
                        .finally(() => {
                            // Reset button state
                            this.disabled = false;
                            this.innerHTML = 'Save All Medications';
                        });
                } catch (error) {
                    console.error('Error with fetch:', error);
                    // If fetch fails, fall back to form submission
                    submitFormFallback();
                    this.disabled = false;
                    this.innerHTML = 'Save All Medications';
                }
            });
        }

        // Fallback function for form submission
        function submitFormFallback() {
            // Clear previous inputs
            const container = document.getElementById('medicationIdsContainer');
            container.innerHTML = '';

            // Add hidden inputs for each medication ID
            selectedMedications.forEach((id, index) => {
                const input = document.createElement('input');
                input.type = 'hidden';
                input.name = `medicationIds[${index}]`;
                input.value = id;
                container.appendChild(input);
            });

            // Submit the form
            document.getElementById('saveMedicationsForm').submit();
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

                // Get all medications from the page
                const allMedications = {};
                document.querySelectorAll('.add-medication-btn').forEach(button => {
                    const id = parseInt(button.getAttribute('data-medication-id'));
                    const name = button.getAttribute('data-medication-name');
                    const price = button.getAttribute('data-medication-price');
                    allMedications[id] = { name, price };
                });

                // Add each medication to the list
                selectedMedications.forEach(medicationId => {
                    const medication = allMedications[medicationId];
                    if (!medication) return;

                    const row = document.createElement('tr');
                    row.innerHTML = `
                        <td>${medication.name}</td>
                        <td>${medication.price}</td>
                        <td>
                            <input type="number" class="form-control form-control-sm" value="1" min="1">
                        </td>
                        <td>
                            <div class="form-check">
                                <input class="form-check-input" type="checkbox" checked>
                            </div>
                        </td>
                        <td class="text-end">
                            <button type="button" class="btn btn-sm btn-outline-danger remove-medication-btn" data-medication-id="${medicationId}">
                                <i class="bi bi-trash"></i>
                            </button>
                        </td>
                    `;
                    selectedMedicationsList.appendChild(row);

                    // Add event listener to the remove button
                    const removeButton = row.querySelector('.remove-medication-btn');
                    removeButton.addEventListener('click', function () {
                        const id = parseInt(this.getAttribute('data-medication-id'));
                        selectedMedications = selectedMedications.filter(medId => medId !== id);
                        updateSelectedMedicationsList();
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
        // ... other medication details
    };

    if (viewDetailsButtons.length > 0 && medicationDetailsModal) {
        viewDetailsButtons.forEach(button => {
            button.addEventListener('click', function () {
                const medicationId = this.getAttribute('data-medication-id');

                // Try to get details from the page first
                const cardEl = this.closest('.card');
                let name, category, price;

                if (cardEl) {
                    name = cardEl.querySelector('.card-title')?.textContent;
                    category = cardEl.querySelector('.text-muted.small')?.textContent;
                    price = cardEl.querySelector('.fw-bold')?.textContent;
                } else {
                    // Try to get from table row
                    const rowEl = this.closest('tr');
                    if (rowEl) {
                        name = rowEl.querySelector('.fw-medium')?.textContent;
                        const cells = rowEl.querySelectorAll('td');
                        if (cells.length >= 3) {
                            category = cells[2].textContent;
                            price = cells[3].textContent;
                        }
                    }
                }

                // Fallback to mock data if needed
                const details = medicationDetails[medicationId] || {
                    name: name || 'Unknown Medication',
                    category: category || 'Unknown Category',
                    price: price || '0.00 EGP',
                    description: 'No description available.'
                };

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
            });
        });

        // Add medication from modal
        if (modalAddMedicationBtn) {
            modalAddMedicationBtn.addEventListener('click', function () {
                const medicationId = parseInt(this.getAttribute('data-medication-id'));
                const medicationName = this.getAttribute('data-medication-name');

                // Check if medication is already in the array
                if (!selectedMedications.some(id => id === medicationId)) {
                    selectedMedications.push(medicationId);
                    updateSelectedMedicationsList();
                    alert(`${medicationName} added to your selection!`);

                    // Close the modal
                    const modal = bootstrap.Modal.getInstance(medicationDetailsModal);
                    modal.hide();
                } else {
                    alert(`${medicationName} is already in your selection!`);
                }
            });
        }
    }
});