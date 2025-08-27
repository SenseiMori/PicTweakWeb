import React, { useMemo } from 'react';
import ImageCard from './ImageCard';
import { generateImageKey } from '../Utils/ImageUtils';

/**
 * Grid component for displaying images
 * @param {Object} props
 * @param {Array} props.images - Array of image objects
 * @param {Function} props.onDeleteImage - Delete handler function
 */
const ImageGrid = ({ images, onDeleteImage }) => {
    // Memoize the grid to prevent unnecessary re-renders
    const imageGrid = useMemo(() => (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {images.map((image, index) => (
                <ImageCard
                    key={generateImageKey(image, index)}
                    image={image}
                    onDelete={onDeleteImage}
                />
            ))}
        </div>
    ), [images, onDeleteImage]);

    if (images.length === 0) {
        return (
            <div className="text-center py-8 text-gray-500">
                <p className="text-lg">Изображения не найдены</p>
                <p className="text-sm">Загрузите первое изображение выше</p>
            </div>
        );
    }

    return imageGrid;
};

export default ImageGrid;
