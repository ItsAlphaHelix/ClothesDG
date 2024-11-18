const rangeInput = document.querySelectorAll(".range-input input"),
    range = document.querySelector(".slider .progress"),
    minPriceDisplay = document.getElementById("minPriceDisplay"),
    maxPriceDisplay = document.getElementById("maxPriceDisplay"),
    minPriceTag = document.getElementById("minPriceTag"),
    maxPriceTag = document.getElementById("maxPriceTag");

let priceGap = 0;

function updatePriceTags() {
    let minVal = parseInt(rangeInput[0].value),
        maxVal = parseInt(rangeInput[1].value),
        minRangeMax = parseInt(rangeInput[0].max);

    // Позициониране на ценовите етикети
    const minPercent = (minVal / minRangeMax) * 100;
    const maxPercent = (maxVal / minRangeMax) * 100;

    minPriceTag.style.left = minPercent + "%";
    maxPriceTag.style.left = maxPercent + "%";

    // Обновяване на стойностите
    minPriceTag.textContent = minVal + " лв.";
    maxPriceTag.textContent = maxVal + " лв.";

    // Обновяване на прогрес бара
    range.style.left = minPercent + "%";
    range.style.right = 100 - maxPercent + "%";
}

rangeInput.forEach((input) => {
    input.addEventListener("input", (e) => {
        let minVal = parseInt(rangeInput[0].value),
            maxVal = parseInt(rangeInput[1].value);

        if (maxVal - minVal < priceGap) {
            if (e.target.className === "range-min") {
                rangeInput[0].value = maxVal - priceGap;
            } else {
                rangeInput[1].value = minVal + priceGap;
            }
        }
        updatePriceTags();
    });
});

// Initialize the values on page load
document.addEventListener("DOMContentLoaded", () => {
    updatePriceTags();
});