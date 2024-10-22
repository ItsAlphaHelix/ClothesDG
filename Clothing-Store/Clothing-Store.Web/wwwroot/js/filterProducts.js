function getSelectedFromURL(param) {
    const params = new URLSearchParams(window.location.search);
    const selectedParam = params.get(param);
    return selectedParam ? selectedParam.split(',') : [];
}

// Initialize selected sizes from the URL
let selectedSizes = getSelectedFromURL('selectedSizes');

// Function to toggle checkbox behavior for the link
function toggleCheckbox(event, item) {
    const link = event.target;
    const params = new URLSearchParams(window.location.search); // Get current query params
    const index = selectedSizes.indexOf(item);

    if (index === -1) {
        // If the item is not selected, add it to the list
         selectedSizes.push(item);
         link.classList.add('active');
    } else {
        // If the item is already selected, remove it
        selectedSizes.splice(index, 1);
        link.classList.remove('active');
    }

     //Update selectedSizes in the URL parameters

    if (selectedSizes.length > 0) {
        params.set('selectedSizes', selectedSizes.join(','));
    } else {
        params.delete('selectedSizes');
    }

    const queryString = params.toString();

    // Redirect: If there's no query string, go to the base URL
    if (queryString) {
        window.location.href = decodeURIComponent(window.location.pathname + '?' + queryString);
    } else {
        window.location.href = window.location.pathname; // No query params, just the base URL
    }
}

let typingTimer;                // Timer identifier
const typingDelay = 500;         // Delay in ms (500ms delay after user stops typing)

// Function to handle price range update
function updatePriceRange() {
    const minPrice = document.getElementById("minPriceTag").textContent.split(' ')[0];
    const maxPrice = document.getElementById("maxPriceTag").textContent.split(' ')[0];

    const params = new URLSearchParams(window.location.search); // Get current query params

    // Update or add the price range parameters in the URL

    params.set("minPrice", minPrice);
    params.set("maxPrice", maxPrice);

    // Redirect with the updated query string, preserving selectedProducts and selectedSizes if present
    window.location.href = window.location.pathname + "?" + params.toString();
}

// Function to delay execution of the price range update
function triggerUpdatePriceRange() {
    clearTimeout(typingTimer); // Clear the previous timer
    typingTimer = setTimeout(updatePriceRange, typingDelay); // Start a new timer
}

document.getElementById("minRange").addEventListener("input", triggerUpdatePriceRange);
document.getElementById("maxRange").addEventListener("input", triggerUpdatePriceRange);


function callChangeOrder(sorting) {
    const params = new URLSearchParams(window.location.search); // Get current query params

    // Set the new sorting parameter
    params.set('sorting', sorting);

    // Redirect with the updated query string
    window.location.href = window.location.pathname + '?' + params.toString();
}