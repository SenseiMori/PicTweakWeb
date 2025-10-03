import React from 'react';

/**
 * Error message component
 * @param {Object} props
 * @param {string} props.error - Error message
 * @param {Function} props.onDismiss - Dismiss handler
 */
const ErrorMessage = ({ error, onDismiss }) => (
    <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-4 flex justify-between items-start">
        <div className="flex items-start">
            <span className="text-red-500 mr-2">❌</span>
            <div>
                <strong>Ошибка:</strong> {error}
            </div>
        </div>
        {onDismiss && (
            <button
                onClick={onDismiss}
                className="text-red-500 hover:text-red-700 font-bold ml-4 focus:outline-none focus:ring-2 focus:ring-red-500 rounded"
                aria-label="Закрыть сообщение об ошибке"
            >
                ✕
            </button>
        )}
    </div>
);

export default ErrorMessage;
