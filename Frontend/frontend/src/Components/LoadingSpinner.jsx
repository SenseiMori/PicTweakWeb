import React from 'react';

/**
 * Loading spinner component
 */
const LoadingSpinner = () => (
    <div className="text-center py-4" role="status" aria-label="Загрузка">
        <div className="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        <p className="mt-2 text-gray-600">Загрузка изображений...</p>
    </div>
);

export default LoadingSpinner;
