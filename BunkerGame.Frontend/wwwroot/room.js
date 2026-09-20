document.addEventListener('DOMContentLoaded', function() {
    const loadingOverlay = document.querySelector('.loading-overlay');
    const roomId = localStorage.getItem('currentRoomId');
    const sessionCode = localStorage.getItem('currentRoomCode');
    const playerNickname = localStorage.getItem('currentPlayerNickname');

    if (!roomId || !sessionCode || !playerNickname) {
        alert('Информация о комнате не найдена. Пожалуйста, вернитесь в главное меню.');
        window.location.href = 'index.html';
        return;
    }

    
    // Initialize room
    document.getElementById('sessionCode').textContent = sessionCode;
    document.getElementById('playerNickname').textContent = playerNickname;
    
    // Hide loading overlay
    setTimeout(() => {
        loadingOverlay.classList.remove('active');
    }, 1000);

    // Back button
    document.getElementById('backBtn').addEventListener('click', () => {
        localStorage.removeItem('currentRoomId');
        localStorage.removeItem('currentRoomCode');
        localStorage.removeItem('currentPlayerNickname');
        window.location.href = 'index.html';
    });

    // Load room info
    loadRoomInfo();

    // Auto-refresh room info every 5 seconds
    setInterval(loadRoomInfo, 5000);

    async function loadRoomInfo() {
        try {
            const response = await fetch(`${API_CONFIG.BASE_URL}/api/Room/GetRoomInfo?roomId=${roomId}&currentNickname=${encodeURIComponent(playerNickname)}`);
            
            if (response.ok) {
                const data = await response.json();
                const room = data.data;
                
                updateRoomDisplay(room);
                updatePlayersDisplay(room.players);
                updateGameControls(room);
                
                // Check for active voting
                if (room.status === 'Playing') {
                    await loadVotingInfo();
                }
            } else {
                console.error('Не удалось загрузить информацию о комнате');
            }
        } catch (error) {
            console.error('Ошибка при загрузке информации о комнате:', error);
        }
    }

    function updateRoomDisplay(room) {
        document.getElementById('roomName').textContent = room.name;
        document.getElementById('roomStatus').textContent = room.status;
        document.getElementById('playerCount').textContent = room.currentPlayers;
        document.getElementById('maxPlayers').textContent = room.maxPlayers;
        document.getElementById('storyText').textContent = room.story || 'Нет доступных историй.';
    }

    function updatePlayersDisplay(players) {
        const playersList = document.getElementById('playersList');
        playersList.innerHTML = '';
        const currentNickname = localStorage.getItem('currentPlayerNickname');

        players.forEach(player => {
            const isCurrent = player.nickname === currentNickname;
            const playerCard = document.createElement('div');
            playerCard.className = `player-card ${player.isAlive ? 'alive' : 'eliminated'} ${player.isWinner ? 'winner' : ''}${isCurrent ? ' current-player' : ''}`;
            
            let characteristics = '';
            if (isCurrent) {
                characteristics += `<div class=\"current-player-badge\">YOU</div>`;
            }
            characteristics += `<div class=\"player-name\">${player.nickname}</div>`;
            
            // Always show profession
            if (player.profession) {
                characteristics += `<div class="characteristic">👨‍💼 Profession: ${player.profession}</div>`;
            }
            
            // Show other characteristics only if revealed
            if (player.gender && player.isGenderRevealed) {
                characteristics += `<div class="characteristic">👤 Пол: ${player.gender}</div>`;
            }
            if (player.age && player.isAgeRevealed) {
                characteristics += `<div class="characteristic">🎂 Возраст: ${player.age}</div>`;
            }
            if (player.orientation && player.isOrientationRevealed) {
                characteristics += `<div class="characteristic">💕 Ориентация: ${player.orientation}</div>`;
            }
            if (player.hobby && player.isHobbyRevealed) {
                characteristics += `<div class="characteristic">🎯 Хобби: ${player.hobby}</div>`;
            }
            if (player.phobia && player.isPhobiaRevealed) {
                characteristics += `<div class="characteristic">😱 Фобия: ${player.phobia}</div>`;
            }
            if (player.luggage && player.isLuggageRevealed) {
                characteristics += `<div class="characteristic">🎒 Багаж: ${player.luggage}</div>`;
            }
            if (player.additionalInformation && player.isAdditionalInfoRevealed) {
                characteristics += `<div class="characteristic">ℹ️ Дополнительная информация: ${player.additionalInformation}</div>`;
            }
            if (player.bodyType && player.isBodyTypeRevealed) {
                characteristics += `<div class="characteristic">💪 Тип тела: ${player.bodyType}</div>`;
            }
            if (player.health && player.isHealthRevealed) {
                characteristics += `<div class="characteristic">🏥 Здоровье: ${player.health}</div>`;
            }
            if (player.personality && player.isPersonalityRevealed) {
                characteristics += `<div class="characteristic">🧠 Личность: ${player.personality}</div>`;
            }
            
            playerCard.innerHTML = characteristics;
            playersList.appendChild(playerCard);
        });
    }

    function updateGameControls(room) {
        const startGameBtn = document.getElementById('startGameBtn');
        const startVotingBtn = document.getElementById('startVotingBtn');
        const revealCharacteristicBtn = document.getElementById('revealCharacteristicBtn');

        if (room.status === 'Ожидание') {
            startGameBtn.style.display = 'block';
            startVotingBtn.style.display = 'none';
            revealCharacteristicBtn.style.display = 'block';
        } else if (room.status === 'В игре') {
            startGameBtn.style.display = 'none';
            startVotingBtn.style.display = 'block';
            revealCharacteristicBtn.style.display = 'block';
        } else {
            startGameBtn.style.display = 'none';
            startVotingBtn.style.display = 'none';
            revealCharacteristicBtn.style.display = 'none';
        }
    }

    // Start Game
    document.getElementById('startGameBtn').addEventListener('click', async () => {
        try {
            const response = await fetch(`${API_CONFIG.BASE_URL}/api/Room/StartGame`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ roomId: parseInt(roomId) })
            });

            if (response.ok) {
                loadRoomInfo(); // Refresh room info
            } else {
                alert('Ошибка при запуске игры');
            }
        } catch (error) {
            alert('Ошибка сервера');
        }
    });

    // Start Voting
    document.getElementById('startVotingBtn').addEventListener('click', async () => {
        try {
            const response = await fetch(`${API_CONFIG.BASE_URL}/api/Room/StartVoting`, {
                method: 'POST',
                headers: { 'Тип контента': 'application/json' },
                body: JSON.stringify({ roomId: parseInt(roomId) })
            });

            if (response.ok) {
                await loadVotingInfo();
            } else {
                alert('Не удалось начать голосование');
            }
        } catch (error) {
            alert('Ошибка сервера');
        }
    });

    // Reveal Characteristic
    document.getElementById('revealCharacteristicBtn').addEventListener('click', () => {
        showRevealModal();
    });

    function showRevealModal() {
        const modal = document.getElementById('revealModal');
        const playerSelect = document.getElementById('playerSelect');
        const characteristicSelect = document.getElementById('characteristicSelect');
        const currentNickname = localStorage.getItem('currentPlayerNickname');
        // Populate player select (only current player)
        playerSelect.innerHTML = '';
        const option = document.createElement('option');
        option.value = currentNickname;
        option.textContent = currentNickname;
        playerSelect.appendChild(option);
        // Fetch room and player data
        fetch(`${API_CONFIG.BASE_URL}/api/Room/GetRoomInfo?roomId=${roomId}&currentNickname=${encodeURIComponent(currentNickname)}`)
            .then(res => res.json())
            .then(data => {
                const player = data.data.players.find(p => p.nickname === currentNickname);
                // List of all characteristics with values
                const allCharacteristics = [
                    { value: 'gender', label: 'Gender', revealed: player.isGenderRevealed, val: player.gender },
                    { value: 'age', label: 'Age', revealed: player.isAgeRevealed, val: player.age },
                    { value: 'orientation', label: 'Orientation', revealed: player.isOrientationRevealed, val: player.orientation },
                    { value: 'hobby', label: 'Hobby', revealed: player.isHobbyRevealed, val: player.hobby },
                    { value: 'phobia', label: 'Phobia', revealed: player.isPhobiaRevealed, val: player.phobia },
                    { value: 'luggage', label: 'Luggage', revealed: player.isLuggageRevealed, val: player.luggage },
                    { value: 'additionalinfo', label: 'Additional Information', revealed: player.isAdditionalInfoRevealed, val: player.additionalInformation },
                    { value: 'bodytype', label: 'Body Type', revealed: player.isBodyTypeRevealed, val: player.bodyType },
                    { value: 'health', label: 'Health', revealed: player.isHealthRevealed, val: player.health },
                    { value: 'personality', label: 'Personality', revealed: player.isPersonalityRevealed, val: player.personality }
                ];
                // Оставляем только нераскрытые
                characteristicSelect.innerHTML = '<option value="">Select characteristic</option>';
                allCharacteristics.forEach(c => {
                    if (!c.revealed) {
                        const opt = document.createElement('option');
                        opt.value = c.value;
                        opt.textContent = `${c.label} - ${c.val !== undefined && c.val !== null && c.val !== '' ? c.val : 'нет данных'}`;
                        characteristicSelect.appendChild(opt);
                    }
                });
            });
        modal.style.display = 'flex';
    }

    // Close reveal modal
    document.getElementById('closeRevealModal').addEventListener('click', () => {
        document.getElementById('revealModal').style.display = 'none';
    });

    // Reveal form submission
    document.getElementById('revealForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        
        const playerName = document.getElementById('playerSelect').value;
        const characteristic = document.getElementById('characteristicSelect').value;
        
        if (!playerName || !characteristic) {
            document.getElementById('revealMessage').textContent = 'Пожалуйста, выберите и игрока,  и характеристику';
            return;
        }

        try {
            // Find player ID by name
            const response = await fetch(`${API_CONFIG.BASE_URL}/api/Room/GetRoomInfo?roomId=${roomId}&currentNickname=${encodeURIComponent(playerName)}`);
            const data = await response.json();
            const player = data.data.players.find(p => p.nickname === playerName);
            
            if (!player) {
                document.getElementById('revealMessage').textContent = 'Игрок не найден';
                return;
            }

            const revealResponse = await fetch(`${API_CONFIG.BASE_URL}/api/Room/RevealCharacteristic`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ 
                    roomPlayerId: player.id, 
                    characteristic: characteristic 
                })
            });

            if (revealResponse.ok) {
                document.getElementById('revealModal').style.display = 'none';
                loadRoomInfo(); // Refresh to show revealed characteristic
            } else {
                document.getElementById('revealMessage').textContent = 'Не удалось раскрыть характеристику';
            }
        } catch (error) {
            document.getElementById('revealMessage').textContent = 'Ошибка сервера';
        }
    });

    async function loadVotingInfo() {
        try {
            const response = await fetch(`${API_CONFIG.BASE_URL}/api/Room/GetCurrentVoting?roomId=${roomId}`);
            
            if (response.ok) {
                const data = await response.json();
                const voting = data.data;
                
                if (voting) {
                    updateVotingDisplay(voting);
                } else {
                    document.getElementById('votingSection').style.display = 'none';
                }
            }
        } catch (error) {
            console.error('Ошибка при загрузке информации о голосовании:', error);
        }
    }

    function updateVotingDisplay(voting) {
        const votingSection = document.getElementById('votingSection');
        const votingRoundNumber = document.getElementById('votingRoundNumber');
        const votingStatus = document.getElementById('votingStatus');
        const votingOptions = document.getElementById('votingOptions');
        const votingResults = document.getElementById('votingResults');

        votingSection.style.display = 'block';
        votingRoundNumber.textContent = voting.roundNumber;
        
        if (voting.status === 'Active') {
            votingStatus.innerHTML = '<div class="voting-active">🗳️ </div>';
            votingOptions.style.display = 'block';
            votingResults.style.display = 'none';
            
            // Show voting options (alive players)
            showVotingOptions();
        } else {
            votingStatus.innerHTML = '<div class="voting-finished">✅ Голосование завершено</div>';
            votingOptions.style.display = 'none';
            votingResults.style.display = 'block';
            
            // Show voting results
            showVotingResults(voting);
        }
    }

    function showVotingOptions() {
        const votingOptions = document.getElementById('votingOptions');
        votingOptions.innerHTML = '<h4>Голосовать за исключение:</h4>';
        
        // Get alive players
        const alivePlayers = Array.from(document.querySelectorAll('.player-card.alive'))
            .map(card => card.querySelector('.player-name').textContent);
        
        alivePlayers.forEach(playerName => {
            const button = document.createElement('button');
            button.className = 'vote-btn';
            button.textContent = playerName;
            button.onclick = () => castVote(playerName);
            votingOptions.appendChild(button);
        });
    }

    async function castVote(targetPlayerName) {
        try {
            // Get current voting round
            const votingResponse = await fetch(`${API_CONFIG.BASE_URL}/api/Room/GetCurrentVoting?roomId=${roomId}`);
            const votingData = await votingResponse.json();
            const voting = votingData.data;
            
            if (!voting) {
                alert('Нет активного голосования');
                return;
            }

            // Find player IDs
            const roomResponse = await fetch(`${API_CONFIG.BASE_URL}/api/Room/GetRoomInfo?roomId=${roomId}&currentNickname=${encodeURIComponent(playerNickname)}`);
            const roomData = await roomResponse.json();
            const voter = roomData.data.players.find(p => p.nickname === playerNickname);
            const target = roomData.data.players.find(p => p.nickname === targetPlayerName);
            
            if (!voter || !target) {
                alert('Игрок не найден');
                return;
            }

            const voteResponse = await fetch(`${API_CONFIG.BASE_URL}/api/Room/Vote`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    votingRoundId: voting.id,
                    voterId: voter.id,
                    votedForId: target.id
                })
            });

            if (voteResponse.ok) {
                alert('Голос успешно отдан');
                loadVotingInfo(); // Refresh voting info
            } else {
                alert('Не удалось отдать голос');
            }
        } catch (error) {
            alert('Ошибка сервера');
        }
    }

    function showVotingResults(voting) {
        const votingResults = document.getElementById('votingResults');
        
        if (voting.eliminatedPlayerNickname) {
            votingResults.innerHTML = `
                <div class="eliminated-player">
                    <h4>Устранен: ${voting.eliminatedPlayerNickname}</h4>
                </div>
            `;
        }
        
        if (voting.votes && voting.votes.length > 0) {
            const votesList = document.createElement('div');
            votesList.innerHTML = '<h4>Голоса:</h4>';
            
            voting.votes.forEach(vote => {
                const voteItem = document.createElement('div');
                voteItem.className = 'vote-item';
                voteItem.textContent = `${vote.voterNickname} проголосовал за ${vote.votedForNickname}`;
                votesList.appendChild(voteItem);
            });
            
            votingResults.appendChild(votesList);
        }
    }

    // Close modal when clicking outside
    window.addEventListener('click', (event) => {
        const modal = document.getElementById('revealModal');
        if (event.target === modal) {
            modal.style.display = 'none';
        }
    });
}); 