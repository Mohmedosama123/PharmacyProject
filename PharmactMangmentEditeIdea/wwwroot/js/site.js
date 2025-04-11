// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Theme toggle functionality
document.addEventListener("DOMContentLoaded", () => {
    const themeToggle = document.getElementById("theme-toggle");
    const lightIcon = document.getElementById("light-icon");
    const darkIcon = document.getElementById("dark-icon");

    // Check for saved theme preference or use system preference
    const savedTheme = localStorage.getItem("theme");
    const systemPrefersDark = window.matchMedia("(prefers-color-scheme: dark)").matches;

    if (savedTheme === "dark" || (!savedTheme && systemPrefersDark)) {
        document.body.classList.add("dark-mode");
        lightIcon.classList.add("d-none");
        darkIcon.classList.remove("d-none");
    }

    // Toggle theme when button is clicked
    if (themeToggle) {
        themeToggle.addEventListener("click", () => {
            document.body.classList.toggle("dark-mode");
            lightIcon.classList.toggle("d-none");
            darkIcon.classList.toggle("d-none");

            // Save preference to localStorage
            if (document.body.classList.contains("dark-mode")) {
                localStorage.setItem("theme", "dark");
            } else {
                localStorage.setItem("theme", "light");
            }
        });
    }

    // Handle navbar background on scroll
    const navbar = document.querySelector(".navbar-container");
    const isHomePage = window.location.pathname === "/" || window.location.pathname.endsWith("index.html");

    if (navbar && isHomePage) {
        window.addEventListener("scroll", () => {
            if (window.scrollY > 10) {
                navbar.classList.add("scrolled");
            } else {
                navbar.classList.remove("scrolled");
            }
        });

        // Initial check
        if (window.scrollY > 10) {
            navbar.classList.add("scrolled");
        }
    }

    // Animation on scroll
    const animateElements = document.querySelectorAll('.animate-on-scroll');

    if (animateElements.length > 0) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const delay = entry.target.getAttribute('data-delay') || 0;
                    setTimeout(() => {
                        entry.target.classList.add('animated');
                    }, delay);
                }
            });
        }, { threshold: 0.1 });

        animateElements.forEach(el => observer.observe(el));
    }

    // Voice search functionality
    const voiceSearchBtn = document.getElementById('voice-search-btn');
    const medicationInput = document.getElementById('medication');

    if (voiceSearchBtn && medicationInput && 'webkitSpeechRecognition' in window) {
        const recognition = new webkitSpeechRecognition();
        recognition.continuous = false;
        recognition.interimResults = false;
        recognition.lang = 'en-US';

        voiceSearchBtn.addEventListener('click', function () {
            recognition.start();
            voiceSearchBtn.classList.add('listening');
        });

        recognition.onresult = function (event) {
            const transcript = event.results[0][0].transcript;
            medicationInput.value = transcript;
            voiceSearchBtn.classList.remove('listening');
        };

        recognition.onend = function () {
            voiceSearchBtn.classList.remove('listening');
        };

        recognition.onerror = function () {
            voiceSearchBtn.classList.remove('listening');
        };
    } else if (voiceSearchBtn) {
        voiceSearchBtn.disabled = true;
        voiceSearchBtn.title = 'Voice recognition not supported in your browser';
    }

    // Handle location dropdowns
    const governorateSelect = document.getElementById('governorate');
    const districtSelect = document.getElementById('district');
    const centerSelect = document.getElementById('center');

    if (governorateSelect && districtSelect && centerSelect) {
        // Mock data for districts and centers
        const districts = {
            "Cairo": ["Maadi", "Heliopolis", "Nasr City", "Downtown", "Zamalek"],
            "Alexandria": ["Montaza", "Sidi Gaber", "Smouha", "Miami", "Agami"],
            "Giza": ["Dokki", "Mohandessin", "6th of October", "Sheikh Zayed", "Haram"],
            "Luxor": ["East Bank", "West Bank", "Karnak", "New Luxor"],
            "Aswan": ["Aswan City", "Elephantine Island", "Kom Ombo", "Edfu"]
        };

        const centers = {
            "Maadi": ["Maadi Grand Mall", "Carrefour Maadi", "Street 9", "Degla"],
            "Heliopolis": ["Citystars", "Roxy Square", "Korba", "Merghany"],
            "Nasr City": ["City Center", "Wonderland", "Abbas El-Akkad", "Makram Ebeid"],
            "Downtown": ["Tahrir Square", "Talaat Harb", "Abdeen", "Opera"],
            "Zamalek": ["26th of July Corridor", "Brazil Street", "Gezira Club"]
            // Add more as needed
        };

        governorateSelect.addEventListener('change', function () {
            const governorate = this.value;
            districtSelect.disabled = !governorate;
            districtSelect.innerHTML = '<option value="">Select district</option>';
            centerSelect.disabled = true;
            centerSelect.innerHTML = '<option value="">Select district first</option>';

            if (governorate && districts[governorate]) {
                districts[governorate].forEach(district => {
                    const option = document.createElement('option');
                    option.value = district;
                    option.textContent = district;
                    districtSelect.appendChild(option);
                });
            }
        });

        districtSelect.addEventListener('change', function () {
            const district = this.value;
            centerSelect.disabled = !district;
            centerSelect.innerHTML = '<option value="">Select center/area</option>';

            if (district && centers[district]) {
                centers[district].forEach(center => {
                    const option = document.createElement('option');
                    option.value = center;
                    option.textContent = center;
                    centerSelect.appendChild(option);
                });
            }
        });
    }

    // Handle image upload preview
    const imageInput = document.getElementById('ImageFile');
    const imagePreview = document.getElementById('imagePreview');

    if (imageInput && imagePreview) {
        imagePreview.addEventListener('click', function () {
            imageInput.click();
        });

        imageInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                const reader = new FileReader();

                reader.onload = function (e) {
                    imagePreview.innerHTML = `<img src="${e.target.result}" class="img-fluid rounded" alt="Medication Preview">`;
                }

                reader.readAsDataURL(this.files[0]);
            }
        });
    }

    // Handle pharmacy profile image upload
    const profileImageContainer = document.querySelector('.pharmacy-image-container.editable');
    const profileImageInput = document.getElementById('profileImage');

    if (profileImageContainer && profileImageInput) {
        profileImageContainer.addEventListener('click', function () {
            profileImageInput.click();
        });

        profileImageInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                const reader = new FileReader();

                reader.onload = function (e) {
                    const img = profileImageContainer.querySelector('img');
                    img.src = e.target.result;
                }

                reader.readAsDataURL(this.files[0]);
            }
        });
    }

    // Handle medication search
    const medicationSearch = document.getElementById('medicationSearch');
    const medicationRows = document.querySelectorAll('tbody tr');

    if (medicationSearch && medicationRows.length > 0) {
        medicationSearch.addEventListener('input', function () {
            const searchTerm = this.value.toLowerCase();

            medicationRows.forEach(row => {
                const medicationName = row.querySelector('td:nth-child(2)').textContent.toLowerCase();
                if (medicationName.includes(searchTerm)) {
                    row.style.display = '';
                } else {
                    row.style.display = 'none';
                }
            });
        });
    }

    // Handle delete modal
    const deleteMedicationModal = document.getElementById('deleteMedicationModal');
    if (deleteMedicationModal) {
        deleteMedicationModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const medicationId = button.getAttribute('data-medication-id');
            const medicationName = button.getAttribute('data-medication-name');

            document.getElementById('medicationIdToDelete').value = medicationId;
            document.getElementById('medicationNameToDelete').textContent = medicationName;
        });
    }
});



/* dashbord */ 

