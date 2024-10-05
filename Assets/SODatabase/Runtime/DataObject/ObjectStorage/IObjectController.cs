namespace SODatabase.DataObject
{
    /// <summary>
    /// �I�u�W�F�N�g�̑���𐧌䂷�邽�߂̃C���^�[�t�F�[�X�ł��B
    /// <typeparam name="T">���䂷��Ώۂ̃I�u�W�F�N�g�̌^�B<see cref="BaseObject"/> �̔h���N���X�łȂ���΂Ȃ�܂���B</typeparam>
    /// </summary>
    public interface IObjectController<T>
        where T : BaseObject
    {
        /// <summary>
        /// �w�肳�ꂽ�I�u�W�F�N�g���X�V���܂��B
        /// </summary>
        /// <param name="obj">�X�V����Ώۂ� <typeparamref name="T"/> �C���X�^���X�B</param>
        /// <returns>
        /// �I�u�W�F�N�g������ɍX�V���ꂽ�ꍇ�� <c>true</c>�A����ȊO�̏ꍇ�� <c>false</c> ��Ԃ��܂��B
        /// </returns>
        bool UpdateObject(T obj);
    }
}
