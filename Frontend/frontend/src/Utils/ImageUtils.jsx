/**
 * Utility functions for image processing and formatting
 */

/**
 * Normalizes image data from API response to consistent format
 * @param {Object} raw - Raw image data from API
 * @returns {Object|null} Normalized image object or null if invalid
 */
export const normalizeImage = (raw) => {
    if (!raw || typeof raw !== 'object') return null;

    return {
        fileName: raw.fileName || raw.FileName || 'Без названия',
        width: raw.width || raw.Width || null,
        height: raw.height || raw.Height || null,
        size: raw.size || raw.Size || null,
        url: raw.url || raw.Url || null,
        uploadedAt: raw.uploadedAt ? new Date(raw.uploadedAt) : null,
    };
};

/**
 * Formats bytes to human-readable string
 * @param {number} bytes - Size in bytes
 * @returns {string} Formatted size string
 */
export const formatBytes = (bytes) => {
    if (bytes == null || bytes === 0) return bytes === 0 ? '0 B' : '';

    const units = ['B', 'KB', 'MB', 'GB'];
    const k = 1024;
    const i = Math.floor(Math.log(bytes) / Math.log(k));

    return `${(bytes / Math.pow(k, i)).toFixed(2)} ${units[i]}`;
};

/**
 * Formats date to human-readable string
 * @param {Date} date - Date object
 * @returns {string} Formatted date (e.g., "15:00 on June 30th 2025")
 */
export const formatDate = (date) => {
    if (!date) return '';
    return date.toLocaleString('en-US', {
        hour: '2-digit',
        minute: '2-digit',
        day: '2-digit',
        month: 'long',
        year: 'numeric',
        hour12: false,
    }).replace(',', ' on');
};

/**
 * Handles API errors consistently
 * @param {Response} response - Fetch response object
 * @returns {Promise<string>} Error message
 */
export const handleApiError = async (response) => {
    let errorMessage = `HTTP ${response.status}: ${response.statusText}`;

    try {
        const responseText = await response.text();
        if (!responseText) return errorMessage;

        try {
            const errorData = JSON.parse(responseText);
            errorMessage = errorData.error || errorData.message || responseText;
        } catch {
            errorMessage = responseText;
        }
    } catch (textError) {
        console.error('Failed to read response:', textError);
    }

    return errorMessage;
};
