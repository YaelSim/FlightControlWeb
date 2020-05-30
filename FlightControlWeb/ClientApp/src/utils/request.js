/**
 * 
 * @param {string} url 
 * @param {object} options 
 */
export default async function request(url, options) {
    const response = await fetch(url, options);
    if (!response.ok) {
        try {
            throw await response.json();
        } catch {
            throw response;
        }
    }

    return await response.json();
}
