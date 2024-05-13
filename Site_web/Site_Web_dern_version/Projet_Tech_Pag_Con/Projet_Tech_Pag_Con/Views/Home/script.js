function showForm(methodName) {
    var formContainer = document.getElementById('form-container-' + methodName);
    if (formContainer.style.display === 'block') {
        formContainer.style.display = 'none';
    } else {
        formContainer.style.display = 'block';
    }

    // Assure que le formulaire est affiché en bas de la carte
    window.scrollTo({
        top: formContainer.offsetTop,
        behavior: 'smooth'
    });
}