import { useState, useCallback } from 'react';
import { normalizeImage } from '../Utils/ImageUtils';

/**
 * Custom hook for managing image API operations
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
            const response = await fetch('/api/images');

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }

            const data = await response.json();
            const normalizedImages = Array.isArray(data)
                ? data.map(normalizeImage).filter(Boolean)
                : [];

            setImages(normalizedImages);
        } catch (err) {
            console.error('Failed to fetch images:', err);
            setError(err.message || 'Неизвестная ошибка при загрузке');
        } finally {
            setIsLoading(false);
        }
    }, []);

    const deleteImage = useCallback(async (imageId) => {
        if (!imageId) return false;

        try {
            const response = await fetch(`/api/images/${imageId}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                throw new Error(`Ошибка удаления: HTTP ${response.status}`);
            }

            // Optimistic update - remove from state immediately
            setImages(prev => prev.filter(img => img.id !== imageId));
            return true;
        } catch (err) {
            console.error('Failed to delete image:', err);
            setError(err.message || 'Ошибка при удалении изображения');
            return false;
        }
    }, []);

    const clearError = useCallback(() => {
        setError(null);
    }, []);

    return {
        images,
        isLoading,
        error,
        fetchImages,
        deleteImage,
        clearError
    };
};
