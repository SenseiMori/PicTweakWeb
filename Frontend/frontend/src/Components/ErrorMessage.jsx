import React from 'react';

/**
 * Error message component with optional dismiss functionality
 * @param {Object} props
 * @param {string} props.error - Error message to display
 * @param {Function} props.onDismiss - Optional dismiss handler
 */
const ErrorMessage = ({ error, onDismiss }) => (
    <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4 flex justify-between items-center">
        <div>
            <strong>Ошибка:</strong> {error}
        </div>
        {onDismiss && (
            <button
                onClick={onDismiss}
                className="text-red-700 hover:text-red-900 font-bold"
                aria-label="Закрыть сообщение об ошибке"
            >
                ×
            </button>
        )}
    </div>
);

export default ErrorMessage;
