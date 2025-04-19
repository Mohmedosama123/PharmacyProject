document.addEventListener('DOMContentLoaded', function () {
    const chatMessages = document.getElementById('chat-messages');
    const searchBtn = document.getElementById('search-btn');
    const chatInputContainer = document.getElementById('chat-input-container');
    const chatInput = document.getElementById('chat-input');
    const sendBtn = document.getElementById('send-btn');

    let currentStep = 0;
    const steps = [
        { question: "من فضلك، أدخل اسم الدواء الذي تبحث عنه", field: "medicine" },
        { question: "أدخل اسم المحافظة", field: "governorate" },
        { question: "أدخل اسم المنطقة", field: "area" }
    ];
    let userAnswers = {};

    addBotMessage("أهلاً بيك قوللي اسم الدوا وأنا هقولك فين تلاقيه حواليك 👌💊");
    searchBtn.addEventListener('click', function () {
        this.classList.add('hidden');
        chatInputContainer.style.display = 'block';
        startConversation();
    });

    sendBtn.addEventListener('click', sendMessage);

    chatInput.addEventListener('keypress', function (e) {
        if (e.key === 'Enter') {
            sendMessage();
        }
    });

    function startConversation() {
        currentStep = 0;
        userAnswers = {};
        askQuestion();
    }

    function askQuestion() {
        addBotMessage(steps[currentStep].question);
    }

    function sendMessage() {
        const message = chatInput.value.trim();
        if (message === '') return;

        addUserMessage(message);
        chatInput.value = '';

        userAnswers[steps[currentStep].field] = message;

        if (currentStep < steps.length - 1) {
            currentStep++;
            setTimeout(askQuestion, 1000);
        } else {
            setTimeout(finishConversation, 1000);
        }
    }

    function finishConversation() {
        addBotMessage("جاري البحث عن الدواء المطلوبة...");

        // Show typing indicator
        const typingId = showTyping();

        fetchMedicineData(userAnswers.medicine, userAnswers.governorate, userAnswers.area)
            .then(data => {
                removeTyping(typingId);
                displayResults(data);
                endConversation();
            })
            .catch(error => {
                removeTyping(typingId);
                addBotMessage("الدواء غير متوفر في منطقتك");
                console.error("Error fetching medicine data:", error);
                endConversation();
            });
    }

    function endConversation() {
        setTimeout(() => {
            addBotMessage("لو عاوز تعمل بحث مره تانيه اضغط علي زر البحث", true);
            searchBtn.classList.remove('hidden');
            chatInputContainer.style.display = 'none';
        }, 1500);
    }

    function fetchMedicineData(medicine, governorate, area) {
        const encodedMedicine = encodeURIComponent(medicine);
        const encodedGov = encodeURIComponent(governorate);
        const encodedArea = encodeURIComponent(area);

        const apiUrl = `http://127.0.0.1:5000/get-pharmacy?governorate=${encodedGov}&area=${encodedArea}&medicine=${encodedMedicine}`;

        return fetch(apiUrl)
            .then(response => {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.json();
            })
            .then(data => {
                if (!data || data.length === 0) {
                    return null;
                }
                return data;
            });
    }

    function displayResults(data) {
        if (!data || data.length === 0) {
            addBotMessage("الدواء غير متوفر في منطقتك");
            return;
        }

        const pharmacyMap = {};
        data.forEach(medicine => {
            if (!pharmacyMap[medicine.NameOfPharmacy]) {
                pharmacyMap[medicine.NameOfPharmacy] = {
                    pharmacyInfo: {
                        NameOfPharmacy: medicine.NameOfPharmacy,
                        Governorate: medicine.Governorate,
                        Area: medicine.Area,
                        PhoneNumber: medicine.PhoneNumber || "غير متوفر",
                        OperatingHours: medicine.OperatingHours || "غير متوفر",
                        FullAddress: medicine.FullAddress || `${medicine.Governorate} - ${medicine.Area}`
                    },
                    medicines: []
                };
            }
            pharmacyMap[medicine.NameOfPharmacy].medicines.push(medicine);
        });

        const messageDiv = document.createElement('div');
        messageDiv.className = 'message bot-message';
        messageDiv.innerHTML = ` نتائج البحث عن "${userAnswers.medicine}":`;
        chatMessages.appendChild(messageDiv);

        Object.values(pharmacyMap).forEach(pharmacyData => {
            // Pharmacy info
            const pharmacyInfoDiv = document.createElement('div');
            pharmacyInfoDiv.className = 'pharmacy-info';
            pharmacyInfoDiv.innerHTML = `
                <h4><i class="fas fa-clinic-medical"></i> ${pharmacyData.pharmacyInfo.NameOfPharmacy}</h4>
                <p><i class="fas fa-map-marker-alt"></i> المحافظة: ${pharmacyData.pharmacyInfo.Governorate}</p>
                <p><i class="fas fa-map-marked-alt"></i> العنوان: ${pharmacyData.pharmacyInfo.FullAddress}</p>
                <p><i class="fas fa-clock"></i> مواعيد العمل: ${pharmacyData.pharmacyInfo.OperatingHours}</p>
                <p><i class="fas fa-phone"></i> الهاتف: ${pharmacyData.pharmacyInfo.PhoneNumber}</p>
            `;
            chatMessages.appendChild(pharmacyInfoDiv);

            // Medicines list
            pharmacyData.medicines.forEach(medicine => {
                const medicineDiv = document.createElement('div');
                medicineDiv.className = 'medicine-card';
                medicineDiv.innerHTML = `
                    <h5>${medicine.Name}</h5>
                    <p><i class="fas fa-info-circle"></i> ${medicine.Description || "لا يوجد وصف متاح"}</p>
                    ${medicine.Category ? `<span class="medicine-category">${medicine.Category}</span>` : ''}
                    <p class="medicine-price"><i class="fas fa-tag"></i> السعر: ${medicine.Price || "غير متوفر"} جنيه</p>
                `;
                chatMessages.appendChild(medicineDiv);
            });
        });

        const closeDiv = document.createElement('div');
        closeDiv.className = 'quick-action';
        closeDiv.innerHTML = `
            <button class="close-btn" onclick="this.parentElement.parentElement.querySelectorAll('.pharmacy-info, .medicine-card').forEach(el => el.style.display='none'); this.style.display='none'">
                <i class="fas fa-times"></i> إخفاء النتائج
            </button>
        `;
        chatMessages.appendChild(closeDiv);

        // Scroll to bottom
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }

    function addBotMessage(text, withQuickBtn = false) {
        const messageDiv = document.createElement('div');
        messageDiv.className = 'message bot-message';
        messageDiv.textContent = text;
        chatMessages.appendChild(messageDiv);



        chatMessages.scrollTop = chatMessages.scrollHeight;
    }

    function addUserMessage(text) {
        const messageDiv = document.createElement('div');
        messageDiv.className = 'message user-message';
        messageDiv.textContent = text;
        chatMessages.appendChild(messageDiv);
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }

    function showTyping() {
        const typingId = 'typing-' + Date.now();
        const typingDiv = document.createElement('div');
        typingDiv.className = 'message bot-message';
        typingDiv.id = typingId;
        typingDiv.innerHTML = `
            <div class="typing-indicator">
                <div class="typing-dot"></div>
                <div class="typing-dot"></div>
                <div class="typing-dot"></div>
            </div>
        `;
        chatMessages.appendChild(typingDiv);
        chatMessages.scrollTop = chatMessages.scrollHeight;
        return typingId;
    }

    function removeTyping(id) {
        const typingElement = document.getElementById(id);
        if (typingElement) {
            typingElement.remove();
        }
    }

    window.addBotMessage = addBotMessage;
    window.addUserMessage = addUserMessage;
});