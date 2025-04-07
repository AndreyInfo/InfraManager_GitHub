define(['jquery', 'knockout'], ($, ko) => {
    return class ShowEntityService {
        /**
         * 
         * @param {string} mainWrapper - jquery selector
         * @param {string} leftWrapper - jquery selector
         * @param {string} rightWrapper - jquery selector
         * @param {string} controlBlockResize - jquery selector
         * @param {{minWidthLeft: Number, minWidthRight: Number}} settingParamsWidth - width settings object
         */
        constructor(
            mainWrapper,
            leftWrapper,
            rightWrapper,
            controlBlockResize,
            settingParamsWidth)
        {
            this.WRAPPER_CLASS = 'is-not-show-entity';
            this.DEFAULT_PROPERTIES = {
                leftBlockWidth: 'maxWidth',
                rigtBlockWidth: 'width'
            }

            this.mainWrapper = $(mainWrapper);
            this.leftWrapper = $(leftWrapper);
            this.rightWrapper = $(rightWrapper);
            this.controlBlockResize = $(controlBlockResize);
            this.SETTING_PARAMS_WIDTH = settingParamsWidth;

            this.formLastShowId = null;
            this.viewModel = null;
        }

        init(model) {
            this._cleanRightWrapperNode();
            this.viewModel = model;
            this.updateScreen();
            this.resizeHandler();
            ko.applyBindings(model, this.rightWrapper.get(0));
        }

        destroy() {
            this._cleanRightWrapperNode();
            this.controlBlockResize.unbind('mousedown');
            $(document).unbind('mousemove');
            $(document).unbind('mouseup');
            this.resetParamsBlocks();
            this.formLastShowId = null;
        }

        resizeHandler() {
            let isResizing = false;
            this.controlBlockResize.on('mousedown', () => {
                isResizing = true;
            });

            $(document).on('mousemove', (e) => {
                if (!isResizing) return;

                $(document).trigger('resizeEntity');

                const widthRight = this.mainWrapper.width() - (e.clientX - this.mainWrapper.offset().left);
                const widthLeft = $(window).width() - widthRight;
                const isStopResize = () => widthLeft < this.SETTING_PARAMS_WIDTH.minWidthLeft || widthRight < this.SETTING_PARAMS_WIDTH.minWidthRight;
                if (isStopResize()) return;

                this.setNativeWidthBlocks(widthLeft, widthRight);

                return false;
            }).on('mouseup', () => isResizing = false);
        }

        updateScreen() {
            if (this.viewModel && this.viewModel.Visible()) {
                this.resizeHandler();
                this.setWidthBlock();
            } else {
                this.resetParamsBlocks();
            }
        }

        setWidthBlock() {
            const leftWidth = this.mainWrapper.width() - this.SETTING_PARAMS_WIDTH.minWidthRight;
            this.setNativeWidthBlocks(leftWidth, this.SETTING_PARAMS_WIDTH.minWidthRight);
        }

        setNativeWidthBlocks(leftMaxWidth, rightWidth) {
            this.leftWrapper.css('maxWidth', leftMaxWidth);
            this.rightWrapper.css('width', rightWidth);
        }

        resetParamsBlocks() {
            this.mainWrapper.removeClass(this.WRAPPER_CLASS);
            this.leftWrapper.addClass(this.WRAPPER_CLASS);
            this.rightWrapper.css(this.DEFAULT_PROPERTIES.rigtBlockWidth, 0);
            this.leftWrapper.css(this.DEFAULT_PROPERTIES.leftBlockWidth, '100%');
        }

        setLastShowFormId(formId) {
            this.formLastShowId = formId;
        }
        
        _cleanRightWrapperNode() {
            if (this.rightWrapper.length > 0) {
                ko.cleanNode(this.rightWrapper.get(0));
            }
        }
    }
});