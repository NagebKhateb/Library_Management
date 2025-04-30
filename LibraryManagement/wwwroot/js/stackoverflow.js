class StackOverflowViewer {
    constructor() {
        this.questionsContainer = document.getElementById('questionsContainer');
        this.loadingIndicator = document.getElementById('loadingIndicator');
        this.init();
    }

    async init() {
        try {
            await this.fetchQuestions();
            this.setCurrentYear();
        } catch (error) {
            console.error('Error initializing:', error);
            this.showError('Failed to load questions. Please refresh the page.');
        }
    }

    async fetchQuestions() {
        try {
            const response = await fetch('/api/stackoverflow/questions');
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const questions = await response.json();
            this.renderQuestions(questions);
        } catch (error) {
            console.error('Error fetching questions:', error);
            throw error;
        } finally {
            this.loadingIndicator.style.display = 'none';
        }
    }

    renderQuestions(questions) {
        if (!questions || questions.length === 0) {
            this.questionsContainer.innerHTML = '<p>No questions found.</p>';
            return;
        }

        let html = '';
        questions.forEach(question => {
            const date = new Date(question.creation_date * 1000).toLocaleDateString();
            const answerClass = question.is_answered ? 'answered' : '';

            html += `
            <div class="question-card">
                <div class="question-stats">
                    <div class="stat votes">
                        <span>${question.score}</span>
                        <small>votes</small>
                    </div>
                    <div class="stat answers ${answerClass}">
                        <span>${question.answer_count}</span>
                        <small>answers</small>
                    </div>
                </div>

                <h3 class="question-title">
                    <a href="question.html?id=${question.question_id}">
                        ${question.title}
                    </a>
                </h3>

                <div class="tags">
                    ${question.tags?.map(tag => `
                        <span class="tag">${tag}</span>
                    `).join('') ?? ''}
                </div>

                <div class="author-info">
                    <span>asked ${date}</span>
                    ${question.owner ? `
                        <a href="${question.owner.link}" target="_blank">
                            <img src="${question.owner.profile_image}" alt="${question.owner.display_name}" class="avatar">
                        </a>
                    ` : ''}
                </div>
            </div>
        `;
        });

        this.questionsContainer.innerHTML = html;
    }

    showError(message) {
        this.questionsContainer.innerHTML = `
            <div style="text-align: center; color: red; padding: 20px;">
                <i class="fas fa-exclamation-triangle"></i>
                <p>${message}</p>
            </div>
        `;
    }

    setCurrentYear() {
        document.getElementById('currentYear').textContent = new Date().getFullYear();
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new StackOverflowViewer();
});