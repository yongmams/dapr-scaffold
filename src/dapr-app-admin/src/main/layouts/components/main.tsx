
import React from 'react'
import { Layout } from 'antd'
import { connect } from 'react-redux'
import { Outlet, useLocation } from 'react-router-dom'
import { CSSTransition, TransitionGroup } from 'react-transition-group'

const { Content } = Layout

const Main = (props: any) => {
    const location = useLocation()
    const { fixedHeader, tagsView } = props

    return (
        <Content
            className={`${fixedHeader ? 'fixedHeader' : ''} ${tagsView ? 'hasTags' : 'noTags'} `}
        >
            {/* 动画 styles-transition.scss => forward-from-right back-to-right fade-in fade-transform*/}
            <TransitionGroup
                childFactory={child => React.cloneElement(child, { classNames: 'forward-from-right' })}
            >
                <CSSTransition timeout={500} key={location.pathname}>
                        <Outlet />
                </CSSTransition>
            </TransitionGroup>
        </Content>
    )
}

const mapStateToProps = (state: any) => {
    return {
        ...state.settings
    }
}
export default connect(mapStateToProps)(Main)
