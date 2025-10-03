import { useState, useCallback } from 'react';
import { normalizeImage, handleApiError } from '../Utils/ImageUtils';

/**
 * Custom hook for managing image API operations (file system based)
 * @returns {Object} API state and methods
 */
export const useImageAPI = () => {
    const [images, setImages] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchImages = useCallback(async () => {
        setIsLoading(true);
        setError(null);

        try {
            const response = await fetch('/api/images', {
                method: 'GET',
                headers: { 'Accept': 'application/json' },
            });

            if (!response.ok) {
                const errorMessage = await handleApiError(response);
                throw new Error(errorMessage);
            }

            let data;
            try {
                const responseText = await response.text();
                data = responseText ? JSON.parse(responseText) : [];
            } catch (parseError) {
                console.error('Failed to parse JSON:', parseError);
                throw new Error('Некорректный ответ от сервера');
            }

            const normalizedImages = Array.isArray(data)
                ? data.map(normalizeImage).filter(Boolean)
                : [];

            setImages(normalizedImages);
        } catch (err) {
            console.error('Failed to fetch images:', err);
            setError(err.message || 'Ошибка загрузки изображений');
        } finally {
            setIsLoading(false);
        }
    }, []);

    const deleteImage = useCallback(async (fileName) => {
        if (!fileName) return false;

        try {
            const response = await fetch(`/api/images/${encodeURIComponent(fileName)}`, {
                method: 'DELETE',
                headers: { 'Accept': 'application/json' },
            });

            if (!response.ok) {
                const errorMessage = await handleApiError(response);
                throw new Error(errorMessage);
            }

            // Optimistic update
            setImages((prev) => prev.filter((img) => img.fileName !== fileName));
            return true;
        } catch (err) {
            console.error('Failed to delete image:', err);
            setError(err.message || 'Ошибка удаления изображения');
            return false;
        }
    }, []);

    const clearError = useCallback(() => setError(null), []);

    return { images, isLoading, error, fetchImages, deleteImage, clearError };
};
