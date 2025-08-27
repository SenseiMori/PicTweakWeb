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
        id: raw.id || raw.Id || null,
        url: raw.url || raw.Url || raw.filePath || raw.FilePath || null,
        fileName: raw.fileName || raw.FileName || raw.imageName || 'Без названия',
        width: raw.width || raw.Width || null,
        height: raw.height || raw.Height || null,
        size: raw.size || raw.Size || raw.fileSize || null,
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
 * Generates a unique key for image components
 * @param {Object} image - Image object
 * @param {number} index - Fallback index
 * @returns {string} Unique key
 */
export const generateImageKey = (image, index = 0) => {
    return image.id || `${image.url}_${image.fileName}_${index}`;
};