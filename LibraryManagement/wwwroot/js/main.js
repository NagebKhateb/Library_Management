class CategoryManager {
    constructor() {
        this.mainCategorySelect = document.getElementById('mainCategory');
        this.subCategorySelect = document.getElementById('subCategory');
        this.filterBtn = document.getElementById('filterBtn');
        this.booksList = document.getElementById('booksList');

        this.init();
    }

    init() {
        if (!this.mainCategorySelect) return;

        this.fetchCategories();
        this.setupEventListeners();
    }

    async fetchCategories() {
        try {
            const response = await fetch('/api/sub-category/by-main-category');
            if (!response.ok) throw new Error('Network response was not ok');

            const categories = await response.json();
            this.populateMainCategories(categories);
        } catch (error) {
            console.error('Error fetching categories:', error);
            this.showError('Failed to load categories. Please try again later.');
        }
    }

    populateMainCategories(categories) {
        this.mainCategorySelect.innerHTML = '<option value="" disabled selected>Select a category</option>';

        categories.forEach(category => {
            const option = new Option(
                category.categoryName,
                category.categoryId
            );
            option.dataset.subCategories = JSON.stringify(category.subCategories);
            this.mainCategorySelect.add(option);
        });
    }

    setupEventListeners() {
        this.mainCategorySelect.addEventListener('change', () => this.handleMainCategoryChange());
        this.filterBtn.addEventListener('click', () => this.handleFilterClick());
    }

    handleMainCategoryChange() {
        const selectedOption = this.mainCategorySelect.options[this.mainCategorySelect.selectedIndex];
        const subCategories = JSON.parse(selectedOption.dataset.subCategories || '[]');

        this.subCategorySelect.disabled = subCategories.length === 0;
        this.subCategorySelect.innerHTML = '<option value="" disabled selected>Select a subcategory</option>';

        subCategories.forEach(subCat => {
            const option = new Option(
                subCat.subCategoryName,
                subCat.subCategoryId
            );
            this.subCategorySelect.add(option);
        });
    }

    async handleFilterClick() {
        const mainCatId = this.mainCategorySelect.value;
        const subCatId = this.subCategorySelect.value;

        if (!mainCatId || !subCatId) {
            this.showError('Please select both a category and subcategory');
            return;
        }

        try {
            const books = await this.fetchBooks(mainCatId, subCatId);
            this.displayBooks(books);
        } catch (error) {
            console.error('Error filtering books:', error);
            this.showError('Failed to apply filters. Please try again.');
        }
    }

    async fetchBooks(categoryId, subCategoryId) {
        const response = await fetch(`/api/book/featch?categoryId=${categoryId}&subCategoryId=${subCategoryId}`);
        if (!response.ok) throw new Error('Network response was not ok');
        return await response.json();
    }

    displayBooks(books) {
        this.booksList.innerHTML = '';

        if (books.length === 0) {
            this.booksList.innerHTML = '<li class="no-books">No books found for this filter.</li>';
            return;
        }

        books.forEach(book => {
            const li = document.createElement('li');
            li.innerHTML = `
                <strong>${book.title}</strong>
                <p>${book.description}</p>
                ${book.author ? `<span class="author">By ${book.author}</span>` : ''}
            `;
            this.booksList.appendChild(li);
        });

        document.getElementById('booksSection').scrollIntoView({ behavior: 'smooth' });
    }

    showError(message) {
        alert(message); // يمكن استبدال هذا بعرض رسالة أكثر جمالاً في الواجهة
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new CategoryManager();
});