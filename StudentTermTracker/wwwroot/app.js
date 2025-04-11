// Custom JavaScript functions for interop with Blazor

/**
 * Scrolls the currently focused input element into view
 * with an additional offset to ensure visibility above the keyboard
 */
window.scrollInputIntoView = function () {
    setTimeout(function () {
        // get active/focused element
        const activeElement = document.activeElement;

        if (activeElement) {
            // calc position to scroll to (element position - offset)
            // offset gives space above the keyboard
            const rect = activeElement.getBoundingClientRect();
            const scrollOffset = 100; // TODO: Adjust this value after testing as needed.

            window.scrollTo({
                top: window.scrollY + rect.top - scrollOffset,
                behavior: 'smooth'
            });
        }
    }, 300); // Small delay to ensure the keyboard has appeared
};