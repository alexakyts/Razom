const header = document.querySelector("header");

window.addEventListener("scroll", function () {
    header.classList.toggle("sticky", window.scrollY > 0);

});

function toggleAuthenticationElements(isAuthenticated) {
    var loginLink = document.getElementById("login");
    var logoutLink = document.getElementById("logout");
    var profileLink = document.getElementById("profile");

    if (isAuthenticated) {
        loginLink.style.display = "none";
        logoutLink.style.display = "block";
        profileLink.style.display = "block";
    } else {
        loginLink.style.display = "block";
        logoutLink.style.display = "none";
        profileLink.style.display = "none";
    }
}

// Example function to simulate user authentication status
function checkUserAuthenticationStatus() {
    // Replace this with your actual authentication status checking logic
    var isAuthenticated = true; // Assuming user is authenticated for this example
    return isAuthenticated;
}

// Function to handle click on login link
function handleLoginClick(event) {
    // Prevent default behavior of anchor tag
    event.preventDefault();
    // Add your logic to handle login action here
    console.log("Login clicked");
}

// Function to handle click on logout link
function handleLogoutClick(event) {
    // Prevent default behavior of anchor tag
    event.preventDefault();
    // Add your logic to handle logout action here
    console.log("Logout clicked");
}

// Function to handle click on profile link
function handleProfileClick(event) {
    // Prevent default behavior of anchor tag
    event.preventDefault();
    // Add your logic to handle profile action here
    console.log("Profile clicked");
}

// Add event listeners to login, logout, and profile links
document.getElementById("login").addEventListener("click", handleLoginClick);
document.getElementById("logout").addEventListener("click", handleLogoutClick);
document.getElementById("profile").addEventListener("click", handleProfileClick);

// Initial setup based on user authentication status
toggleAuthenticationElements(checkUserAuthenticationStatus());