class QuestionDetailViewer {
    constructor() {
        this.questionDetails = document.getElementById('questionDetails');
        this.loadingIndicator = document.getElementById('loadingIndicator');
        this.errorDisplay = document.getElementById('errorDisplay');
        this.retryButton = document.getElementById('retryButton');

        this.questionId = this.getQuestionIdFromUrl();

        if (this.questionId) {
            this.init();
        } else {
            this.showError('No question ID provided in URL');
        }
    }

    init() {
        this.setupEventListeners();
        this.fetchQuestionDetails();
        this.setCurrentYear();
    }

    setupEventListeners() {
        this.retryButton.addEventListener('click', () => {
            this.fetchQuestionDetails();
        });
    }

    getQuestionIdFromUrl() {
        const params = new URLSearchParams(window.location.search);
        return params.get('id');
    }

    async fetchQuestionDetails() {
        this.showLoading();
        this.hideError();

        try {
            // In a real app, you would call your backend API:
            // const response = await fetch(`/api/stackoverflow/questions/${this.questionId}`);

            // For demo purposes, we'll call StackOverflow API directly:
            const response = await fetch(
                `https://api.stackexchange.com/2.3/questions/${this.questionId}?order=desc&sort=activity&site=stackoverflow&filter=withbody`
            );

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();

            if (data.items && data.items.length > 0) {
                this.renderQuestion(data.items[0]);
            } else {
                throw new Error('Question not found');
            }
        } catch (error) {
            console.error('Error fetching question details:', error);
            this.showError('Failed to load question details. Please try again later.');
        } finally {
            this.hideLoading();
        }
    }

    renderQuestion(question) {
        if (!question) {
            this.questionDetails.innerHTML = '<p class="error">Question data is not available.</p>';
            return;
        }

        // تحسين تنسيق التاريخ مع معالجة القيم غير المعرفة
        const dateOptions = { year: 'numeric', month: 'short', day: 'numeric' };
        const safeDate = (timestamp) => {
            if (!timestamp) return 'N/A';
            return new Date(timestamp * 1000).toLocaleDateString('en-US', dateOptions);
        };

        const creationDate = safeDate(question.creation_date);
        const lastActivityDate = safeDate(question.last_activity_date);

        const tagsHtml = question.tags?.length > 0 ?
            question.tags.map(tag => `<span class="tag">${tag || ''}</span>`).join('') :
            '<span class="no-tags">No tags</span>';

        const answersHtml = question.answer_count > 0 ?
            (question.answers?.map(answer => this.renderAnswer(answer))?.join('') || '') :
            '<p class="no-answers">No answers yet.</p>';

        let ownerHtml = '';
        if (question.owner) {
            ownerHtml = `
            <div class="author-card">
                <span class="asked-date">asked ${creationDate}</span>
                <div class="author-details">
                    <a href="${question.owner.link || '#'}" target="_blank" rel="noopener" class="author">
                        ${question.owner.profile_image ?
                    `<img src="${question.owner.profile_image}" alt="${question.owner.display_name || 'user'}" class="avatar">` :
                    '<div class="avatar-placeholder"><i class="fas fa-user"></i></div>'
                }
                        <span class="author-name">${question.owner.display_name || 'Anonymous'}</span>
                    </a>
                    ${question.owner.reputation ?
                    `<span class="reputation">${question.owner.reputation}</span>` :
                    ''
                }
                </div>
            </div>
        `;
        }

        const questionHtml = `
        <article class="question-full">
            <header class="question-header">
                <h1 class="question-title">${question.title || 'No title'}</h1>
                
                <div class="question-meta">
                    <div class="meta-item">
                        <span class="meta-label">Asked</span>
                        <span class="meta-value">${creationDate}</span>
                    </div>
                    <div class="meta-item">
                        <span class="meta-label">Active</span>
                        <span class="meta-value">${lastActivityDate}</span>
                    </div>
                    <div class="meta-item">
                        <span class="meta-label">Viewed</span>
                        <span class="meta-value">${question.view_count || 0} times</span>
                    </div>
                </div>
            </header>
            
            <div class="question-content">
                <div class="question-body">${question.body || 'No content available.'}</div>
                
                <div class="question-tags">
                    ${tagsHtml}
                </div>
                
                <div class="question-author">
                    ${ownerHtml}
                </div>
            </div>
        </article>
        
        <section class="answers-section">
            <h2 class="answers-title">
                ${question.answer_count || 0} ${question.answer_count === 1 ? 'Answer' : 'Answers'}
            </h2>
            
            ${answersHtml}
        </section>
    `;

        this.questionDetails.innerHTML = questionHtml;
    }

    renderAnswer(answer) {
        const creationDate = new Date(answer.creation_date * 1000).toLocaleDateString();
        const isAccepted = answer.is_accepted;

        return `
            <article class="answer ${isAccepted ? 'accepted' : ''}">
                <div class="answer-header">
                    <div class="answer-status">
                        ${isAccepted ? `
                            <span class="accepted-badge">
                                <i class="fas fa-check"></i> Accepted
                            </span>
                        ` : ''}
                        <span class="votes ${answer.score < 0 ? 'negative' : ''}">
                            ${answer.score} votes
                        </span>
                    </div>
                    
                    ${answer.owner ? `
                        <div class="answer-author">
                            <span class="answered-date">answered ${creationDate}</span>
                            <a href="${answer.owner.link}" target="_blank" rel="noopener" class="author">
                                <img src="${answer.owner.profile_image}" alt="${answer.owner.display_name}" class="avatar">
                                <span>${this.escapeHtml(answer.owner.display_name)}</span>
                            </a>
                        </div>
                    ` : ''}
                </div>
                
                <div class="answer-body">${answer.body}</div>
                
                <div class="answer-actions">
                    <button class="btn btn-outline">
                        <i class="fas fa-thumbs-up"></i> Upvote
                    </button>
                    <button class="btn btn-outline">
                        <i class="fas fa-comment"></i> Comment
                    </button>
                </div>
            </article>
        `;
    }

    // Helper method to prevent XSS
    escapeHtml(unsafe) {
        if (!unsafe) return '';
        return unsafe.toString()
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    showLoading() {
        this.loadingIndicator.classList.remove('hidden');
        this.questionDetails.classList.add('hidden');
    }

    hideLoading() {
        this.loadingIndicator.classList.add('hidden');
        this.questionDetails.classList.remove('hidden');
    }

    showError(message) {
        this.errorDisplay.classList.remove('hidden');
        this.errorDisplay.querySelector('.error-message').textContent = message;
        this.questionDetails.classList.add('hidden');
        this.hideLoading();
    }

    hideError() {
        this.errorDisplay.classList.add('hidden');
    }

    setCurrentYear() {
        document.getElementById('currentYear').textContent = new Date().getFullYear();
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new QuestionDetailViewer();
});